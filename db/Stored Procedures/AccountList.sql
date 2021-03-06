﻿
CREATE PROC AccountList
    @UserID INT,
    @AccountID INT = NULL,
    @NonZeroBalanceOnly BIT = 0
AS

    SET NOCOUNT ON

    DECLARE @Accounts TABLE (
        ID INT,
        Name NVARCHAR(64),
        Type INT,
        StartingBalance DECIMAL(18,2),
        CurrentBalance DECIMAL(18,2),
        IsIncludedInNetWorth BIT,
        IsDormant BIT,
        LatestMonthlyBudgetID INT DEFAULT 0,
        BalanceAtStartOfMonthlyBudget DECIMAL(18,2) DEFAULT 0,
        DisplayOrder INT,
        NumberLast4Digits NVARCHAR(4)
    )

    INSERT INTO
        @Accounts
    SELECT
        a.ID,
        a.Name,
        a.Type,
        a.StartingBalance,
        a.StartingBalance + ISNULL(SUM(e.Amount), 0) AS CurrentBalance,
        a.IsIncludedInNetWorth,
        a.IsDormant,
        0 AS LatestMonthlyBudgetID,
        0 AS BalanceAtStartOfMonthlyBudget,
        a.DisplayOrder,
        a.NumberLast4Digits
    FROM
        Accounts a
    LEFT JOIN
        Entries e ON e.AccountID = a.ID
    WHERE
        a.UserID = @UserID
    AND
        a.ID = ISNULL(@AccountID, a.ID)
    GROUP BY
        a.ID,
        a.Name,
        a.Type,
        a.StartingBalance,
        a.IsIncludedInNetWorth,
        a.IsDormant,
        a.DisplayOrder,
        a.NumberLast4Digits
    ORDER BY
        a.DisplayOrder

    IF @NonZeroBalanceOnly = 1
        SELECT * FROM @Accounts WHERE CurrentBalance != 0 ORDER BY DisplayOrder
    ELSE
        SELECT * FROM @Accounts ORDER BY DisplayOrder

