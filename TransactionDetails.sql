USE [TransactionDetails]
GO
/****** Object:  UserDefinedFunction [dbo].[SplitString]    Script Date: 01-12-2022 22:47:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SplitString]
(    
    @Input NVARCHAR(MAX),
    @Character CHAR(1)
)
RETURNS @Output TABLE (
    Item NVARCHAR(1000)
)
AS
BEGIN
    DECLARE @StartIndex INT, @EndIndex INT
 
    SET @StartIndex = 1
    IF SUBSTRING(@Input, LEN(@Input) - 1, LEN(@Input)) <> @Character
    BEGIN
        SET @Input = @Input + @Character
    END
 
    WHILE CHARINDEX(@Character, @Input) > 0
    BEGIN
        SET @EndIndex = CHARINDEX(@Character, @Input)
         
        INSERT INTO @Output(Item)
        SELECT SUBSTRING(@Input, @StartIndex, @EndIndex - 1)
         
        SET @Input = SUBSTRING(@Input, @EndIndex + 1, LEN(@Input))
    END
 
    RETURN
END
GO
/****** Object:  Table [dbo].[Account]    Script Date: 01-12-2022 22:47:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[FinancialYearCode] [char](2) NOT NULL,
	[AccountCode] [char](8) NOT NULL,
	[AccountID] [char](15) NULL,
	[AccountName] [char](60) NOT NULL,
	[AccountGroupCode] [smallint] NULL,
	[ExceptGroupCode] [smallint] NULL,
	[GroupCompany] [bit] NOT NULL,
	[Relation] [varchar](30) NULL,
	[OpeningBalance] [float] NULL,
	[ClosingBalance] [float] NULL,
	[CurrencyCode] [smallint] NULL,
	[AccountAddress1] [varchar](50) NULL,
	[AccountAddress2] [varchar](50) NULL,
	[AccountAddress3] [varchar](50) NULL,
	[AccountPin] [varchar](7) NULL,
	[AccountPhone] [varchar](45) NULL,
	[AccountFax] [varchar](45) NULL,
	[AccountResidencePhone] [varchar](45) NULL,
	[AccountHandPhone] [varchar](45) NULL,
	[AccountEMail] [varchar](50) NULL,
	[InterestRate] [numeric](18, 0) NULL,
	[InterestMethod] [char](1) NULL,
	[BankAccountName] [varchar](40) NULL,
	[BankAccountNumber] [varchar](25) NULL,
	[IncomeTaxNumber] [varchar](50) NULL,
	[BranchCode] [char](3) NULL,
	[AccountField1] [varchar](50) NULL,
	[AccountField2] [varchar](50) NULL,
	[AccountField3] [varchar](50) NULL,
	[AccountField4] [varchar](50) NULL,
	[AccountField5] [varchar](50) NULL,
	[ModuleID] [char](4) NULL,
	[Table1Code] [int] NULL,
	[Table2Code] [int] NULL,
	[Suspended] [char](1) NULL,
	[CityCode] [int] NULL,
	[UserCode] [smallint] NULL,
	[EntryDate] [datetime] NULL,
	[ModiUserCode] [smallint] NULL,
	[ModiDate] [datetime] NULL,
	[Field1] [int] NULL,
	[Field2] [int] NULL,
	[AccountField6] [varchar](50) NULL,
	[AccountField7] [varchar](50) NULL,
	[AccountField8] [varchar](50) NULL,
	[AccountField9] [varchar](50) NULL,
	[AccountField10] [varchar](50) NULL,
	[Transfer] [char](1) NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY NONCLUSTERED 
(
	[FinancialYearCode] ASC,
	[AccountCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BatchProcessRecords]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BatchProcessRecords](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[ProcessedRowCount] [int] NOT NULL,
	[CreatedOn] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobBillingDet]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobBillingDet](
	[SerialNumber] [int] NOT NULL,
	[ItemSerialNumber] [smallint] NOT NULL,
	[JDSerialNumber] [int] NOT NULL,
	[JDItemSerialNumber] [smallint] NOT NULL,
	[BillingQuantity] [float] NULL,
	[NoChargeQuantity] [float] NULL,
	[BillingType] [char](1) NULL,
	[BillingRate] [float] NULL,
	[BankCashFlag] [char](1) NULL,
	[TotalAmount] [float] NULL,
	[DiscountPerAmt] [char](1) NULL,
	[DiscountRate] [float] NULL,
	[DiscountAmount] [float] NULL,
	[BillingAmount] [float] NULL,
	[Remarks] [varchar](250) NULL,
	[Addless1] [float] NULL,
	[Addless2] [float] NULL,
	[Addless3] [float] NULL,
	[NetAmount] [float] NULL,
 CONSTRAINT [PK_JobBillingDet] PRIMARY KEY NONCLUSTERED 
(
	[SerialNumber] ASC,
	[ItemSerialNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobBillingMas]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobBillingMas](
	[SerialNumber] [int] NOT NULL,
	[ProcessingPointCode] [char](2) NULL,
	[FinancialYearCode] [char](2) NULL,
	[BranchCode] [char](3) NULL,
	[AccountCode] [char](8) NULL,
	[ReferenceDate] [datetime] NULL,
	[Remarks] [varchar](250) NULL,
	[ReferenceNumber] [varchar](25) NULL,
	[LedSerialNumber] [int] NULL,
	[SaleAccountCode] [char](8) NULL,
	[CreditDays] [smallint] NULL,
	[PostingDate] [datetime] NULL,
	[PostingAmount] [float] NULL,
	[PostingAmount2] [float] NULL,
	[DeletedFlag] [bit] NULL,
	[UserCode] [smallint] NULL,
	[EntryDate] [datetime] NULL,
	[ModiUserCode] [smallint] NULL,
	[ModiDate] [datetime] NULL,
	[ReferenceType] [char](1) NOT NULL,
	[Addless1] [float] NULL,
	[Addless2] [float] NULL,
	[Addless3] [float] NULL,
	[PostingAmount3] [float] NULL,
	[RoundedAmount] [float] NULL,
	[FinalAmount] [float] NULL,
 CONSTRAINT [PK_JobBillingMas] PRIMARY KEY NONCLUSTERED 
(
	[SerialNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobDespatchDet]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobDespatchDet](
	[SerialNumber] [int] NOT NULL,
	[ItemSerialNumber] [smallint] NOT NULL,
	[JRSerialNumber] [int] NOT NULL,
	[JRItemSerialNumber] [smallint] NOT NULL,
	[ItemPieces] [float] NULL,
	[ItemCarats] [float] NULL,
	[ItemLines] [float] NULL,
	[WeightLoss] [float] NULL,
	[PacketStatus] [char](1) NULL,
	[Remarks] [varchar](250) NULL,
	[BillingQuantity] [float] NULL,
	[NoChargeQuantity] [float] NULL,
	[BillingRate] [float] NULL,
 CONSTRAINT [PK_JobDespatchDet] PRIMARY KEY NONCLUSTERED 
(
	[SerialNumber] ASC,
	[ItemSerialNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobDespatchMas]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobDespatchMas](
	[SerialNumber] [int] NOT NULL,
	[ProcessingPointCode] [char](2) NULL,
	[FinancialYearCode] [char](2) NULL,
	[BranchCode] [char](3) NULL,
	[AccountCode] [char](8) NULL,
	[ReferenceDate] [datetime] NULL,
	[Remarks] [varchar](250) NULL,
	[DeletedFlag] [bit] NULL,
	[UserCode] [smallint] NULL,
	[EntryDate] [datetime] NULL,
	[ModiUserCode] [smallint] NULL,
	[ModiDate] [datetime] NULL,
	[ReferenceNumber] [varchar](25) NULL,
 CONSTRAINT [PK_JobDespatchMas] PRIMARY KEY NONCLUSTERED 
(
	[SerialNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobReceiptDet]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobReceiptDet](
	[SerialNumber] [int] NOT NULL,
	[ItemSerialNumber] [smallint] NOT NULL,
	[ProcessCode] [smallint] NOT NULL,
	[PacketNumber] [smallint] NOT NULL,
	[ItemPieces] [float] NULL,
	[ItemCarats] [float] NULL,
	[ItemLines] [float] NULL,
	[ReWorkingFlag] [bit] NULL,
	[Remarks] [varchar](250) NULL,
	[BillingRate] [float] NULL,
	[BillingQuantity] [float] NULL,
 CONSTRAINT [PK_JobReceiptDet] PRIMARY KEY NONCLUSTERED 
(
	[SerialNumber] ASC,
	[ItemSerialNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobReceiptMas]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobReceiptMas](
	[SerialNumber] [int] NOT NULL,
	[ProcessingPointCode] [char](2) NULL,
	[FinancialYearCode] [char](2) NULL,
	[BranchCode] [char](3) NULL,
	[AccountCode] [char](8) NULL,
	[ReferenceDate] [datetime] NULL,
	[Remarks] [varchar](250) NULL,
	[DeletedFlag] [bit] NULL,
	[UserCode] [smallint] NULL,
	[EntryDate] [datetime] NULL,
	[ModiUserCode] [smallint] NULL,
	[ModiDate] [datetime] NULL,
 CONSTRAINT [PK_JobReceiptMas] PRIMARY KEY NONCLUSTERED 
(
	[SerialNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProcessDet]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProcessDet](
	[ProcessCode] [smallint] NOT NULL,
	[SerialNumber] [smallint] NOT NULL,
	[RangeFrom] [float] NULL,
	[RangeTo] [float] NULL,
	[BillingRate] [float] NULL,
 CONSTRAINT [PK_ProcessDet] PRIMARY KEY NONCLUSTERED 
(
	[ProcessCode] ASC,
	[SerialNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProcessMas]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProcessMas](
	[ProcessCode] [smallint] NOT NULL,
	[ProcessName] [varchar](50) NULL,
	[Remarks] [varchar](250) NULL,
	[BillingType] [char](1) NULL,
	[DeletedFlag] [bit] NULL,
	[UserCode] [smallint] NULL,
	[EntryDate] [datetime] NULL,
	[ModiUserCode] [smallint] NULL,
	[ModiDate] [datetime] NULL,
	[SGSTTaxRate] [float] NULL,
	[CGSTTaxRate] [float] NULL,
	[IGSTTaxRate] [float] NULL,
 CONSTRAINT [PK_ProcessMas] PRIMARY KEY NONCLUSTERED 
(
	[ProcessCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_GroupCompany]  DEFAULT ((0)) FOR [GroupCompany]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_OpeningBalance]  DEFAULT ((0)) FOR [OpeningBalance]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_ClosingBalance]  DEFAULT ((0)) FOR [ClosingBalance]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_CurrencyCode]  DEFAULT ((0)) FOR [CurrencyCode]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Suspended]  DEFAULT ('N') FOR [Suspended]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_CityCode]  DEFAULT ((0)) FOR [CityCode]
GO
ALTER TABLE [dbo].[Account] ADD  DEFAULT ('Y') FOR [Transfer]
GO
ALTER TABLE [dbo].[BatchProcessRecords] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[JobBillingMas] ADD  CONSTRAINT [DF_JobBillingMas_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[JobBillingMas] ADD  DEFAULT ('B') FOR [ReferenceType]
GO
ALTER TABLE [dbo].[JobDespatchDet] ADD  CONSTRAINT [DF_JobDespatchMas_PacketStatus]  DEFAULT ('Y') FOR [PacketStatus]
GO
ALTER TABLE [dbo].[JobDespatchMas] ADD  CONSTRAINT [DF_JobDespatchMas_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[JobReceiptDet] ADD  CONSTRAINT [DF_JobReceiptMas_ReWorkingFlag]  DEFAULT ((0)) FOR [ReWorkingFlag]
GO
ALTER TABLE [dbo].[JobReceiptMas] ADD  CONSTRAINT [DF_JobReceiptMas_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[ProcessMas] ADD  CONSTRAINT [DF_ProcessMas_DeletedFlag]  DEFAULT ((0)) FOR [DeletedFlag]
GO
ALTER TABLE [dbo].[ProcessMas] ADD  DEFAULT ((0)) FOR [SGSTTaxRate]
GO
ALTER TABLE [dbo].[ProcessMas] ADD  DEFAULT ((0)) FOR [CGSTTaxRate]
GO
ALTER TABLE [dbo].[ProcessMas] ADD  DEFAULT ((0)) FOR [IGSTTaxRate]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCustomers]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetCustomers]
As  
	Select   
		DISTINCT JM.AccountCode,
		 AC.AccountName
	FROM JobReceiptMas JM
	 LEFT JOIN Account AC
	  ON JM.AccountCode = AC.AccountCode   
GO
/****** Object:  StoredProcedure [dbo].[sp_GetDespatchData]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetDespatchData]
@AccountCode varchar(50)
As  
	SELECT 
		RDT.SerialNumber, RDT.ItemSerialNumber, RDT.ProcessCode, RDT.PacketNumber, RDT.ItemPieces, RDT.ItemCarats, RDT.ItemLines,
		DDt.ItemPieces [JDItemPieces], DDt.ItemCarats [JDItemCarats], DDT.ItemLines [JDItemLines], DDT.WeightLoss,
		DDT.PacketStatus, DDT.BillingQuantity, DDT.NoChargeQuantity, DDT.BillingRate, DDT.Remarks

	FROM JobReceiptDet RDT
	 LEFT JOIN JobDespatchDet DDT
	 ON RDT.SerialNumber = DDt.JRItemSerialNumber
	 LEFT JOIN JobReceiptMas RDM
	 ON RDM.SerialNumber = RDT.SerialNumber
	WHERE RDM.AccountCode =@AccountCode
GO
/****** Object:  StoredProcedure [dbo].[sp_GetNewDespatchData]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetNewDespatchData]
@Input NVARCHAR(MAX)
As  
	SELECT 
		RDT.SerialNumber, RDT.ItemSerialNumber, RM.ReferenceDate , RDT.ProcessCode, RDT.PacketNumber, RDT.ItemPieces, RDT.ItemCarats, RDT.ItemLines,
		DDt.ItemPieces [JDItemPieces], DDt.ItemCarats [JDItemCarats], DDT.ItemLines [JDItemLines], DDT.WeightLoss,
		DDT.PacketStatus, PM.BillingType [Billing Unit], DDT.BillingQuantity, DDT.NoChargeQuantity, DDT.BillingRate, DDT.Remarks

	FROM JobReceiptDet RDT
	 LEFT JOIN JobDespatchDet DDT
	 ON RDT.SerialNumber = DDt.JRSerialNumber AND RDT.ItemSerialNumber =  DDt.JRItemSerialNumber
	 LEFT JOIN ProcessMas PM
	 ON RDT.ProcessCode = PM.ProcessCode
	  LEFT JOIN JobReceiptMas RM
	 ON RDT.SerialNumber = RM.SerialNumber
	WHERE RM.AccountCode=@Input AND RDT.SerialNumber NOT IN (Select JRSerialNumber FROM JobDespatchDet)
GO
/****** Object:  StoredProcedure [dbo].[sp_GetProcess]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetProcess]
As  
	Select   
		 ProcessCode,
		 ProcessName,
		 Remarks,
		 BillingType,
		 DeletedFlag,
		 UserCode,
		 EntryDate,
		 ModiUserCode,
		 ModiDate,
		 SGSTTaxRate,
		 CGSTTaxRate,
		 IGSTTaxRate
	FROM ProcessMas ORDER BY ProcessName
GO
/****** Object:  StoredProcedure [dbo].[sp_GetSearchData]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetSearchData]
As  
	Select   
		DISTINCT Ac.AccountCode,
		 AC.AccountName,
		 JM.ReferenceDate,
		 JM.SerialNumber
	FROM JobReceiptMas JM
	 LEFT JOIN Account AC
	  ON JM.AccountCode = AC.AccountCode   
GO
/****** Object:  StoredProcedure [dbo].[sp_GetSelectedData]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetSelectedData]
@SerialNumber Bigint
As  
	Select   
		 PM.ProcessCode,
		 JD.ItemSerialNumber,
		 JD.PacketNumber,
		 JD.ItemPieces,
		 JD.ItemCarats,
		 JD.ItemLines,
		 JD.Remarks
	FROM JobReceiptDet JD
	 LEFT JOIN ProcessMas PM
	  ON JD.ProcessCode = PM.ProcessCode   
	WHERE JD.SerialNumber =@SerialNumber
GO
/****** Object:  StoredProcedure [dbo].[sp_GetSelectedDespatchData]    Script Date: 01-12-2022 22:47:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetSelectedDespatchData]
@Input NVARCHAR(MAX)
As  
	SELECT 
		RDT.SerialNumber, RDT.ItemSerialNumber, RM.ReferenceDate , RDT.ProcessCode, RDT.PacketNumber, RDT.ItemPieces, RDT.ItemCarats, RDT.ItemLines,
		DDt.ItemPieces [JDItemPieces], DDt.ItemCarats [JDItemCarats], DDT.ItemLines [JDItemLines], DDT.WeightLoss,
		DDT.PacketStatus, PM.BillingType [Billing Unit], DDT.BillingQuantity, DDT.NoChargeQuantity, DDT.BillingRate, DDT.Remarks

	FROM JobReceiptDet RDT
	 LEFT JOIN JobDespatchDet DDT
	 ON RDT.SerialNumber = DDt.JRSerialNumber AND RDT.ItemSerialNumber =  DDt.JRItemSerialNumber
	 LEFT JOIN ProcessMas PM
	 ON RDT.ProcessCode = PM.ProcessCode
	  LEFT JOIN JobReceiptMas RM
	 ON RDT.SerialNumber = RM.SerialNumber
	WHERE RDT.SerialNumber IN(SELECT CAST(Item AS INTEGER)
        FROM dbo.SplitString(@Input, ','))
GO
USE [master]
GO
ALTER DATABASE [TransactionDetails] SET  READ_WRITE 
GO
