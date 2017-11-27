<Query Kind="FSharpProgram">
  <Reference Relative="lib\fsdl.dll">C:\Src\money\tools\lib\fsdl.dll</Reference>
  <Namespace>fsdl</Namespace>
</Query>

let commonColumns = []
let commonConstraints = []

let dtoNamespace = "money.web.Models.DTO"

// Define table specifications
let users = {
    sqlStatementType = CREATE
    tableName = "Users"
    dtoClassName = "UserDTO"
    dtoNamespace = dtoNamespace
    dtoBaseClassName = Some("Abstract.IDTO")
    columnSpecifications = [Identity("ID", INT, 1, 1)
                            NotNull("Email", CHR(256), NONE)
                            NotNull("Password", CHR(2048), NONE)] 
    constraintSpecifications = [PrimaryKey(["ID"])]
    indexSpecifications = []
    addDapperAttributes = true
}

let accounts = {
    sqlStatementType = CREATE
    tableName = "Accounts"
    dtoClassName = "AccountDTO"
    dtoNamespace = dtoNamespace
    dtoBaseClassName = Some("Abstract.IDTO")
    columnSpecifications = [Identity("ID", INT, 1, 1)
                            NotNull("UserID", INT, NONE)
                            NotNull("Name", CHR(64), NONE)
                            NotNull("StartingBalance", MONEY, NONE)
                            NotNull("IsMainAccount", BIT, NONE)
                            NotNull("IsIncludedInNetWorth", BIT, NONE)
                            NotNull("DisplayOrder", INT, NONE)] 
    constraintSpecifications = [PrimaryKey(["ID"])
                                ForeignKey("UserID", "Users", "ID")]
    indexSpecifications = []
    addDapperAttributes = true
}

let categories = {
    sqlStatementType = CREATE
    tableName = "Categories"
    dtoClassName = "CategoryDTO"
    dtoNamespace = dtoNamespace
    dtoBaseClassName = Some("Abstract.IDTO")
    columnSpecifications = [Identity("ID", INT, 1, 1)
                            NotNull("AccountID", INT, NONE)
                            NotNull("Name", CHR(64), NONE)
                            NotNull("DisplayOrder", INT, NONE)] 
    constraintSpecifications = [PrimaryKey(["ID"])
                                ForeignKey("AccountID", "Accounts", "ID")]
    indexSpecifications = []
    addDapperAttributes = true
}

let parties = {
    sqlStatementType = CREATE
    tableName = "Parties"
    dtoClassName = "PartyDTO"
    dtoNamespace = dtoNamespace
    dtoBaseClassName = Some("Abstract.IDTO")
    columnSpecifications = [Identity("ID", INT, 1, 1)
                            NotNull("AccountID", INT, NONE)
                            NotNull("Name", CHR(64), NONE)] 
    constraintSpecifications = [PrimaryKey(["ID"])
                                ForeignKey("AccountID", "Accounts", "ID")]
    indexSpecifications = []
    addDapperAttributes = true
}

let monthlybudgets = {
    sqlStatementType = CREATE
    tableName = "MonthlyBudgets"
    dtoClassName = "MonthlyBudgetDTO"
    dtoNamespace = dtoNamespace
    dtoBaseClassName = Some("Abstract.IDTO")
    columnSpecifications = [Identity("ID", INT, 1, 1)
                            NotNull("AccountID", INT, NONE)
                            NotNull("StartDate", DATE, NONE)
                            NotNull("EndDate", DATE, NONE)] 
    constraintSpecifications = [PrimaryKey(["ID"])
                                ForeignKey("AccountID", "Accounts", "ID")]
    indexSpecifications = [NonClustered(["StartDate"; "EndDate"])
                           NonClustered(["EndDate"; "StartDate"])]
    addDapperAttributes = true
}

let entries = {
    sqlStatementType = CREATE
    tableName = "Entries"
    dtoClassName = "EntryDTO"
    dtoNamespace = dtoNamespace
    dtoBaseClassName = Some("Abstract.IDTO")
    columnSpecifications = [Identity("ID", INT, 1, 1)
                            NotNull("AccountID", INT, NONE)
                            Null("MonthlyBudgetID", INT)
                            Null("CategoryID", INT)
                            Null("PartyID", INT)
                            NotNull("Date", DATE, NONE)
                            NotNull("Amount", MONEY, NONE)
                            Null("Note", CHR(64))
                            Null("TransferGUID", GUID)] 
    constraintSpecifications = [PrimaryKey(["ID"])
                                ForeignKey("AccountID", "Accounts", "ID")
                                ForeignKey("MonthlyBudgetID", "MonthlyBudgets", "ID")
                                ForeignKey("CategoryID", "Categories", "ID")
                                ForeignKey("PartyID", "Parties", "ID")]
    indexSpecifications = []   
    addDapperAttributes = true
}

let categories_monthlybudgets = {
    sqlStatementType = CREATE
    tableName = "Categories_MonthlyBudgets"
    dtoClassName = "Category_MonthlyBudgetDTO"
    dtoNamespace = dtoNamespace
    dtoBaseClassName = None
    columnSpecifications = [NotNull("MonthlyBudgetID", INT, NONE)
                            NotNull("CategoryID", INT, NONE)
                            NotNull("Amount", MONEY, NONE)] 
    constraintSpecifications = [PrimaryKey(["MonthlyBudgetID"; "CategoryID"])
                                ForeignKey("MonthlyBudgetID", "MonthlyBudgets", "ID")
                                ForeignKey("CategoryID", "Categories", "ID")]
    indexSpecifications = []
    addDapperAttributes = true
}

// Add any new table definitions to this list
let tables = [
    users
    accounts
    categories
    parties
    monthlybudgets
    entries
    categories_monthlybudgets
]

let tableSql = fsdl.generateTableDefinitions tables commonColumns
let indexSql = fsdl.generateIndexDefinitions tables
let constraintSql = fsdl.generateConstraintDefinitions tables commonConstraints
let dtoClasses = fsdl.generateDTOClassDefinitionList tables commonColumns

let br = Environment.NewLine
let brbr = br + br

// Make sure we are using this script's path as the working directory
Directory.SetCurrentDirectory (Path.GetDirectoryName Util.CurrentQueryPath)

// Load any extra SQL which needs to be executed before/during/after the generated SQL
let procedureSql = Directory.GetFiles(@"..\db\schema\procedures", "*.sql") 
                   |> Array.map File.ReadAllText 
                   |> String.concat brbr

let schemaSql = sprintf "%s%s%s%s%s%s%s%s%s%s%s" 
                    "CREATE DATABASE [$(DatabaseName)]" brbr 
                    "GO" brbr 
                    "USE [$(DatabaseName)]" brbr 
                    tableSql
                    indexSql
                    constraintSql
                    (sprintf "%s%s%s" procedureSql br "PRINT 'Procedures Created'") br 

// Write the schema file
File.WriteAllText(@"..\db\schema\schema.sql", schemaSql)

let writeFile (name, code) = 
    File.WriteAllText(@"..\money.web\Models\DTO\" + name + ".cs", code)

dtoClasses 
|> List.map writeFile 
|> ignore