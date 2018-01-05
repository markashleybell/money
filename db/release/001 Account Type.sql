/*
   05 January 201807:01:53
   User: 
   Server: localhost
   Database: money
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Accounts
	DROP CONSTRAINT FK_Accounts_UserID_Users_ID
GO
ALTER TABLE dbo.Users SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Accounts
	(
	ID int NOT NULL IDENTITY (1, 1),
	UserID int NOT NULL,
	Name nvarchar(64) NOT NULL,
	Type int NOT NULL,
	StartingBalance decimal(18, 2) NOT NULL,
	IsMainAccount bit NOT NULL,
	IsIncludedInNetWorth bit NOT NULL,
	DisplayOrder int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Accounts SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Accounts ON
GO
IF EXISTS(SELECT * FROM dbo.Accounts)
	 EXEC('INSERT INTO dbo.Tmp_Accounts (ID, UserID, Name, Type, StartingBalance, IsMainAccount, IsIncludedInNetWorth, DisplayOrder)
		SELECT ID, UserID, Name, 0, StartingBalance, IsMainAccount, IsIncludedInNetWorth, DisplayOrder FROM dbo.Accounts WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Accounts OFF
GO
ALTER TABLE dbo.Categories
	DROP CONSTRAINT FK_Categories_AccountID_Accounts_ID
GO
ALTER TABLE dbo.Parties
	DROP CONSTRAINT FK_Parties_AccountID_Accounts_ID
GO
ALTER TABLE dbo.MonthlyBudgets
	DROP CONSTRAINT FK_MonthlyBudgets_AccountID_Accounts_ID
GO
ALTER TABLE dbo.Entries
	DROP CONSTRAINT FK_Entries_AccountID_Accounts_ID
GO
DROP TABLE dbo.Accounts
GO
EXECUTE sp_rename N'dbo.Tmp_Accounts', N'Accounts', 'OBJECT' 
GO
ALTER TABLE dbo.Accounts ADD CONSTRAINT
	PK_Accounts PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Accounts ADD CONSTRAINT
	FK_Accounts_UserID_Users_ID FOREIGN KEY
	(
	UserID
	) REFERENCES dbo.Users
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Entries ADD CONSTRAINT
	FK_Entries_AccountID_Accounts_ID FOREIGN KEY
	(
	AccountID
	) REFERENCES dbo.Accounts
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Entries SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MonthlyBudgets ADD CONSTRAINT
	FK_MonthlyBudgets_AccountID_Accounts_ID FOREIGN KEY
	(
	AccountID
	) REFERENCES dbo.Accounts
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.MonthlyBudgets SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Parties ADD CONSTRAINT
	FK_Parties_AccountID_Accounts_ID FOREIGN KEY
	(
	AccountID
	) REFERENCES dbo.Accounts
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Parties SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Categories ADD CONSTRAINT
	FK_Categories_AccountID_Accounts_ID FOREIGN KEY
	(
	AccountID
	) REFERENCES dbo.Accounts
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Categories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
