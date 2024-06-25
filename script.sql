/****** Object:  UserDefinedFunction [dbo].[CamelCase]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE FUNCTION [dbo].[CamelCase]
(@Str varchar(8000))
RETURNS varchar(8000) AS
BEGIN
  DECLARE @Result varchar(2000)
  SET @Str = LOWER(@Str) + ' '
  SET @Result = ''
  WHILE 1=1
  BEGIN
    IF PATINDEX('% %',@Str) = 0 BREAK
    SET @Result = @Result + UPPER(Left(@Str,1))+
    SubString  (@Str,2,CharIndex(' ',@Str)-1)
    SET @Str = SubString(@Str,
      CharIndex(' ',@Str)+1,Len(@Str))
  END
  SET @Result = RTrim(LTrim(Left(@Result,Len(@Result))))
  RETURN @Result
END




GO
/****** Object:  UserDefinedFunction [dbo].[DateOnly]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create  function [dbo].[DateOnly](@DateTime DateTime)
-- Returns @DateTime at midnight; i.e., it removes the time portion of a DateTime value.
returns datetime
as
    begin
    return dateadd(dd,0, datediff(dd,0,@DateTime))
    end


GO
/****** Object:  UserDefinedFunction [dbo].[DayInMonth]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[DayInMonth]
( 
@startDate date,
@dayOfWeek int,
@weekOffset int
)
Returns date
AS
BEGIN

DECLARE @monthDate date;
DECLARE @dayOffset int;
DECLARE @retValue date;
DECLARE @monthOffset int;
SET @monthOffset = 0;
SET @retValue = '1-1-1990';

IF @weekOffset < 4
BEGIN
	-- First In Month
	WHILE @retValue <= @startDate
	BEGIN
		SET @monthDate = DATEADD(MONTH, DATEDIFF(MONTH, 0, @startDate) + @monthOffset, 0);
		SET @dayOffset = (7 - (DATEPART(weekday, @monthDate) - 1) + @dayOfWeek) % 7;
		SET @retValue = DATEADD(WEEK, @weekOffset, DATEADD(DAY, @dayOffset, @monthDate));
		SET @monthOffset = @monthOffset + 1;
	END
END
ELSE
BEGIN
	-- Last In Month
	WHILE @retValue <= @startDate
	BEGIN
		SET @monthDate = DATEADD(SECOND, -1, DATEADD(MONTH, DATEDIFF(MONTH, 0, @startDate) + 1 + @monthOffset, 0));
		SET @dayOffset = ((0 - (DATEPART(weekday, @monthDate) - 1) + @dayOfWeek) - 7) % 7;
		SET @retValue = DATEADD(DAY, @dayOffset, @monthDate);
		SET @monthOffset = @monthOffset + 1;
	END
END

RETURN  @retValue

END



GO
/****** Object:  UserDefinedFunction [dbo].[ExtractInteger]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[ExtractInteger](@String VARCHAR(2000))
RETURNS VARCHAR(1000)
AS
BEGIN
DECLARE @Count INT
DECLARE @IntNumbers VARCHAR(1000)
SET @Count = 0
SET @IntNumbers = ''

WHILE @Count <= LEN(@String)
BEGIN
IF SUBSTRING(@String,@Count,1) >= '0'
AND SUBSTRING(@String,@Count,1) <= '9'
BEGIN
SET @IntNumbers = @IntNumbers + SUBSTRING(@String,@Count,1)
END
SET @Count = @Count + 1
END

RETURN @IntNumbers
END



GO
/****** Object:  UserDefinedFunction [dbo].[FormatPhone]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[FormatPhone](@String VARCHAR(2000))
RETURNS VARCHAR(1000)
AS
BEGIN
	DECLARE @Count INT;
	DECLARE @IntNumbers VARCHAR(1000);
	DECLARE @Char VARCHAR(1);
	SET @Count = 0;
	SET @IntNumbers = '';

	WHILE @Count <= LEN(@String)
	BEGIN
		SET @Char = SUBSTRING(@String,@Count,1);
		IF @Char != ' ' AND @Char != '-' AND @Char != '(' AND @Char != ')'
		BEGIN
			IF @Char >= '0' AND @Char <= '9'
			BEGIN
				SET @IntNumbers = @IntNumbers + SUBSTRING(@String,@Count,1);
			END
			ELSE
			BEGIN
				return @String;
			END
		END
		SET @Count = @Count + 1;
	END
	
	IF LEN(@IntNumbers) = 10
	BEGIN
		RETURN '(' + SUBSTRING(@IntNumbers,1,3) + ') ' + SUBSTRING(@IntNumbers,4,3) + '-' + SUBSTRING(@IntNumbers,7,4);
	END

	RETURN @String;
END



GO
/****** Object:  UserDefinedFunction [dbo].[InlineMax]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



create function [dbo].[InlineMax](@val1 int, @val2 int)
returns int
as
begin
  if @val1 > @val2
    return @val1
  return isnull(@val2,@val1)
end


GO
/****** Object:  UserDefinedFunction [dbo].[TimeOnly]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



create function [dbo].[TimeOnly](@DateTime DateTime)
-- returns only the time portion of a DateTime, at the "base" date (1/1/1900)
-- Thanks, Peso! 
returns datetime
as
    begin
    return dateadd(day, -datediff(day, 0, @datetime), @datetime)
    end


GO
/****** Object:  Table [dbo].[AppLog]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppLog](
	[dateCreated] [datetime] NULL,
	[version] [varchar](20) NULL,
	[message] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointments](
	[appointmentID] [int] NOT NULL,
	[appStatus] [int] NOT NULL,
	[appType] [int] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[dateUpdated] [datetime] NOT NULL,
	[appointmentDate] [datetime] NOT NULL,
	[startTime] [datetime] NOT NULL,
	[endTime] [datetime] NOT NULL,
	[customerID] [int] NOT NULL,
	[customerHours] [money] NOT NULL,
	[customerRate] [money] NOT NULL,
	[customerServiceFee] [money] NOT NULL,
	[customerSubContractor] [money] NOT NULL,
	[customerDiscountAmount] [money] NOT NULL,
	[customerDiscountPercent] [money] NOT NULL,
	[contractorID] [int] NULL,
	[contractorHours] [money] NOT NULL,
	[contractorRate] [money] NOT NULL,
	[contractorTips] [money] NOT NULL,
	[contractorAdjustAmount] [money] NOT NULL,
	[contractorAdjustType] [varchar](255) NULL,
	[amountPaid] [money] NOT NULL,
	[paymentFinished] [bit] NOT NULL,
	[appointmentStatus] [varchar](50) NULL,
	[recurrenceID] [int] NOT NULL,
	[recurrenceType] [int] NOT NULL,
	[weeklyFrequency] [int] NOT NULL,
	[monthlyWeek] [int] NOT NULL,
	[monthlyDay] [int] NOT NULL,
	[confirmed] [bit] NOT NULL,
	[leftMessage] [bit] NOT NULL,
	[keysReturned] [bit] NOT NULL,
	[followUpSent] [bit] NOT NULL,
	[sentSMS] [bit] NOT NULL,
	[sentWeekSMS] [bit] NOT NULL,
	[sentEmail] [bit] NOT NULL,
	[customerDiscountReferral] [money] NOT NULL,
	[username] [varchar](100) NULL,
	[usernameBooked] [bit] NOT NULL,
	[salesTax] [money] NOT NULL,
 CONSTRAINT [PK_Appointments] PRIMARY KEY CLUSTERED 
(
	[appointmentID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CleaningPacks]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CleaningPacks](
	[cleaningPackID] [int] IDENTITY(5000,1) NOT NULL,
	[customerID] [int] NOT NULL,
	[transType] [varchar](50) NOT NULL,
	[paymentType] [varchar](50) NOT NULL,
	[paymentID] [varchar](50) NULL,
	[lastFourCard] [varchar](16) NULL,
	[batched] [bit] NOT NULL,
	[isVoid] [bit] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[visits] [int] NOT NULL,
	[hoursPerVisit] [money] NOT NULL,
	[serviceFeePerVisit] [money] NOT NULL,
	[ratePerHour] [money] NOT NULL,
	[amount] [money] NOT NULL,
	[points] [money] NOT NULL,
	[email] [varchar](100) NULL,
	[memo] [varchar](500) NULL,
	[username] [varchar](50) NULL,
 CONSTRAINT [PK_CleaningPacks] PRIMARY KEY CLUSTERED 
(
	[cleaningPackID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contractors]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contractors](
	[contractorID] [int] IDENTITY(100500,1) NOT NULL,
	[contractorType] [int] NOT NULL,
	[franchiseMask] [int] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[firstName] [varchar](50) NULL,
	[lastName] [varchar](50) NULL,
	[businessName] [varchar](50) NULL,
	[address] [varchar](50) NULL,
	[city] [varchar](50) NULL,
	[state] [varchar](50) NULL,
	[zip] [varchar](10) NULL,
	[bestPhone] [varchar](15) NULL,
	[alternatePhone] [varchar](15) NULL,
	[email] [varchar](75) NULL,
	[ssn] [varchar](75) NULL,
	[notes] [varchar](max) NULL,
	[paymentType] [varchar](50) NULL,
	[paymentDay] [varchar](50) NULL,
	[hourlyRate] [money] NOT NULL,
	[serviceSplit] [money] NOT NULL,
	[hireDate] [datetime] NOT NULL,
	[accountRep] [bit] NOT NULL,
	[active] [bit] NOT NULL,
	[scheduled] [bit] NOT NULL,
	[sendSchedules] [bit] NOT NULL,
	[lastSchedule] [datetime] NOT NULL,
	[sendPayroll] [bit] NOT NULL,
	[lastPayroll] [datetime] NOT NULL,
	[team] [varchar](50) NOT NULL,
	[score] [money] NOT NULL,
	[startDay] [datetime] NOT NULL,
	[endDay] [datetime] NOT NULL,
	[birthday] [datetime] NOT NULL,
	[waiverDate] [datetime] NOT NULL,
	[waiverUpdateDate] [datetime] NOT NULL,
	[insuranceDate] [datetime] NOT NULL,
	[insuranceUpdateDate] [datetime] NOT NULL,
	[backgroundCheck] [datetime] NOT NULL,
	[applicant] [bit] NOT NULL,
	[apFindUs] [varchar](200) NULL,
	[apWorkedBefore] [varchar](10) NULL,
	[apWorkedBeforeWhen] [varchar](200) NULL,
	[apHowLongAddress] [varchar](50) NULL,
	[apDriversLicense] [varchar](50) NULL,
	[apDriversLicenseExpire] [varchar](50) NULL,
	[apHaveCar] [varchar](10) NULL,
	[apDaysAvailable] [varchar](max) NULL,
	[apHighSchool] [varchar](50) NULL,
	[apCollege] [varchar](50) NULL,
	[apHighSchoolDiploma] [varchar](10) NULL,
	[apFelony] [varchar](10) NULL,
	[apFelonyDescription] [varchar](max) NULL,
	[apRefOneName] [varchar](50) NULL,
	[apRefOnePosition] [varchar](50) NULL,
	[apRefOneCompany] [varchar](50) NULL,
	[apRefOneAddress] [varchar](50) NULL,
	[apRefOnePhoneNumber] [varchar](50) NULL,
	[apRefTwoName] [varchar](50) NULL,
	[apRefTwoPosition] [varchar](50) NULL,
	[apRefTwoCompany] [varchar](50) NULL,
	[apRefTwoAddress] [varchar](50) NULL,
	[apRefTwoPhoneNumber] [varchar](50) NULL,
	[apRelOneName] [varchar](50) NULL,
	[apRelOneRelation] [varchar](50) NULL,
	[apRelOneAddress] [varchar](50) NULL,
	[apRelOnePhoneNumber] [varchar](50) NULL,
	[apRelTwoName] [varchar](50) NULL,
	[apRelTwoRelation] [varchar](50) NULL,
	[apRelTwoAddress] [varchar](50) NULL,
	[apRelTwoPhoneNumber] [varchar](50) NULL,
	[apEmpOneName] [varchar](50) NULL,
	[apEmpOneAddress] [varchar](50) NULL,
	[apEmpOnePhoneNumber] [varchar](50) NULL,
	[apEmpOneSupervisor] [varchar](50) NULL,
	[apEmpOneStartDate] [varchar](50) NULL,
	[apEmpOneEndDate] [varchar](50) NULL,
	[apEmpOneJobTitle] [varchar](50) NULL,
	[apEmpOneReasonLeave] [varchar](400) NULL,
	[apEmpTwoName] [varchar](50) NULL,
	[apEmpTwoAddress] [varchar](50) NULL,
	[apEmpTwoPhoneNumber] [varchar](50) NULL,
	[apEmpTwoSupervisor] [varchar](50) NULL,
	[apEmpTwoStartDate] [varchar](50) NULL,
	[apEmpTwoEndDate] [varchar](50) NULL,
	[apEmpTwoJobTitle] [varchar](50) NULL,
	[apEmpTwoReasonLeave] [varchar](400) NULL,
	[apEmpThreeName] [varchar](50) NULL,
	[apEmpThreeAddress] [varchar](50) NULL,
	[apEmpThreePhoneNumber] [varchar](50) NULL,
	[apEmpThreeSupervisor] [varchar](50) NULL,
	[apEmpThreeStartDate] [varchar](50) NULL,
	[apEmpThreeEndDate] [varchar](50) NULL,
	[apEmpThreeJobTitle] [varchar](50) NULL,
	[apEmpThreeReasonLeave] [varchar](400) NULL,
	[apContactEmployer] [varchar](10) NULL,
 CONSTRAINT [PK_Contractors_1] PRIMARY KEY CLUSTERED 
(
	[contractorID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[customerID] [int] NOT NULL,
	[franchiseMask] [int] NOT NULL,
	[bookedBy] [int] NULL,
	[bookedDate] [datetime] NOT NULL,
	[lastUpdate] [datetime] NOT NULL,
	[accountStatus] [varchar](50) NULL,
	[accountType] [varchar](50) NULL,
	[newBuilding] [bit] NOT NULL,
	[sectionMask] [int] NOT NULL,
	[customNote] [varchar](150) NULL,
	[companyContact] [varchar](100) NULL,
	[firstName] [varchar](50) NULL,
	[lastName] [varchar](50) NULL,
	[spouseName] [varchar](50) NULL,
	[locationAddress] [varchar](225) NULL,
	[locationCity] [varchar](100) NULL,
	[locationState] [varchar](50) NULL,
	[locationZip] [varchar](10) NULL,
	[billingSame] [bit] NOT NULL,
	[billingAddress] [varchar](225) NULL,
	[billingCity] [varchar](100) NULL,
	[billingState] [varchar](50) NULL,
	[billingZip] [varchar](10) NULL,
	[bestPhone] [varchar](50) NULL,
	[alternatePhoneOne] [varchar](50) NULL,
	[alternatePhoneTwo] [varchar](50) NULL,
	[email] [varchar](100) NULL,
	[advertisement] [varchar](255) NULL,
	[paymentType] [varchar](50) NULL,
	[creditCardNumber] [varchar](50) NULL,
	[creditCardExpMonth] [varchar](10) NULL,
	[creditCardExpYear] [varchar](10) NULL,
	[creditCardCCV] [varchar](10) NULL,
	[serviceFee] [money] NOT NULL,
	[ratePerHour] [money] NOT NULL,
	[discountPercent] [money] NOT NULL,
	[discountAmount] [money] NOT NULL,
	[estimatedHours] [varchar](255) NULL,
	[estimatedCC] [varchar](255) NULL,
	[estimatedWW] [varchar](255) NULL,
	[estimatedHW] [varchar](255) NULL,
	[estimatedCL] [varchar](255) NULL,
	[estimatedPrice] [varchar](255) NULL,
	[NC_Notes] [varchar](max) NULL,
	[NC_Special] [varchar](max) NULL,
	[NC_Details] [varchar](max) NULL,
	[NC_FrequencyDay] [varchar](255) NULL,
	[NC_PreferedTime] [varchar](255) NULL,
	[NC_Frequency] [varchar](255) NULL,
	[NC_DayOfWeek] [varchar](255) NULL,
	[NC_TimeOfDayPrefix] [varchar](255) NULL,
	[NC_TimeOfDay] [varchar](255) NULL,
	[NC_Bathrooms] [varchar](255) NULL,
	[NC_Bedrooms] [varchar](255) NULL,
	[NC_SquareFootage] [varchar](255) NULL,
	[NC_Vacuum] [bit] NOT NULL,
	[NC_DoDishes] [bit] NOT NULL,
	[NC_ChangeBed] [bit] NOT NULL,
	[NC_Pets] [varchar](255) NULL,
	[NC_FlooringType] [varchar](255) NULL,
	[NC_FlooringCarpet] [bit] NOT NULL,
	[NC_FlooringHardwood] [bit] NOT NULL,
	[NC_FlooringTile] [bit] NOT NULL,
	[NC_FlooringLinoleum] [bit] NOT NULL,
	[NC_FlooringSlate] [bit] NULL,
	[NC_FlooringMarble] [bit] NOT NULL,
	[NC_EnterHome] [varchar](255) NULL,
	[NC_RequiresKeys] [bit] NOT NULL,
	[NC_Organize] [bit] NOT NULL,
	[NC_CleanRating] [varchar](50) NULL,
	[NC_CleaningType] [varchar](255) NULL,
	[DC_Blinds] [bit] NOT NULL,
	[DC_BlindsAmount] [varchar](255) NULL,
	[DC_BlindsCondition] [varchar](255) NULL,
	[DC_Windows] [bit] NOT NULL,
	[DC_WindowsAmount] [varchar](255) NULL,
	[DC_WindowsSills] [bit] NOT NULL,
	[DC_Walls] [bit] NOT NULL,
	[DC_WallsDetail] [varchar](255) NULL,
	[DC_Baseboards] [bit] NOT NULL,
	[DC_DoorFrames] [bit] NOT NULL,
	[DC_CeilingFans] [bit] NOT NULL,
	[DC_CeilingFansAmount] [varchar](255) NULL,
	[DC_LightFixtures] [bit] NOT NULL,
	[DC_KitchenCuboards] [bit] NOT NULL,
	[DC_KitchenCuboardsDetail] [varchar](255) NULL,
	[DC_BathroomCuboards] [bit] NOT NULL,
	[DC_BathroomCuboardsDetail] [varchar](255) NULL,
	[DC_Oven] [bit] NOT NULL,
	[DC_Refrigerator] [bit] NOT NULL,
	[DC_OtherOne] [varchar](255) NULL,
	[DC_OtherTwo] [varchar](255) NULL,
	[welcomeLetter] [bit] NOT NULL,
	[quoteReply] [bit] NOT NULL,
	[quoteValue] [varchar](max) NULL,
	[DC_LightSwitches] [bit] NOT NULL,
	[DC_VentCovers] [bit] NOT NULL,
	[billingName] [varchar](100) NULL,
	[preferredContact] [varchar](50) NULL,
	[points] [money] NOT NULL,
	[rewardsEnabled] [bit] NOT NULL,
	[referredBy] [int] NOT NULL,
	[bestPhoneCell] [bit] NOT NULL,
	[alternatePhoneOneCell] [bit] NOT NULL,
	[alternatePhoneTwoCell] [bit] NOT NULL,
	[sendPromotions] [bit] NOT NULL,
	[NC_GateCode] [varchar](50) NULL,
	[NC_GarageCode] [varchar](50) NULL,
	[NC_DoorCode] [varchar](50) NULL,
	[NC_RequestEcoCleaners] [bit] NOT NULL,
	[businessName] [varchar](150) NULL,
	[DC_InsideVents] [bit] NOT NULL,
	[DC_Pantry] [bit] NOT NULL,
	[DC_LaundryRoom] [bit] NOT NULL,
	[loginInfoSent] [bit] NOT NULL,
	[CC_SquareFootage] [varchar](50) NULL,
	[CC_RoomCountSmall] [varchar](50) NULL,
	[CC_RoomCountLarge] [varchar](50) NULL,
	[CC_PetOdorAdditive] [bit] NOT NULL,
	[CC_Details] [varchar](max) NULL,
	[WW_BuildingStyle] [varchar](50) NULL,
	[WW_BuildingLevels] [varchar](50) NULL,
	[WW_VaultedCeilings] [bit] NOT NULL,
	[WW_PostConstruction] [bit] NOT NULL,
	[WW_WindowCount] [varchar](50) NULL,
	[WW_WindowType] [varchar](50) NULL,
	[WW_InsidesOutsides] [varchar](50) NULL,
	[WW_Razor] [bit] NOT NULL,
	[WW_RazorCount] [varchar](50) NULL,
	[WW_HardWater] [bit] NOT NULL,
	[WW_HardWaterCount] [varchar](50) NULL,
	[WW_FrenchWindows] [bit] NOT NULL,
	[WW_FrenchWindowCount] [varchar](50) NULL,
	[WW_StormWindows] [bit] NOT NULL,
	[WW_StormWindowCount] [varchar](50) NULL,
	[WW_Screens] [bit] NOT NULL,
	[WW_ScreensCount] [varchar](50) NULL,
	[WW_Tracks] [bit] NOT NULL,
	[WW_TracksCount] [varchar](50) NULL,
	[WW_Wells] [bit] NOT NULL,
	[WW_WellsCount] [varchar](50) NULL,
	[WW_Gutters] [bit] NOT NULL,
	[WW_GuttersFeet] [varchar](50) NULL,
	[WW_Details] [varchar](max) NULL,
	[HW_Frequency] [varchar](50) NULL,
	[HW_StartDate] [varchar](50) NULL,
	[HW_EndDate] [varchar](50) NULL,
	[HW_GarbageCans] [bit] NOT NULL,
	[HW_GarbageDay] [varchar](50) NULL,
	[HW_PlantsWatered] [bit] NOT NULL,
	[HW_PlantsWateredFrequency] [varchar](50) NULL,
	[HW_Thermostat] [bit] NOT NULL,
	[HW_ThermostatTemperature] [varchar](50) NULL,
	[HW_Breakers] [bit] NOT NULL,
	[HW_BreakersLocation] [varchar](50) NULL,
	[HW_CleanBeforeReturn] [bit] NOT NULL,
	[HW_Details] [varchar](max) NULL,
	[reviewUsDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Customers_1] PRIMARY KEY CLUSTERED 
(
	[customerID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DailyTotals]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DailyTotals](
	[franchiseMask] [int] NOT NULL,
	[dataType] [varchar](50) NOT NULL,
	[dataTime] [datetime] NOT NULL,
	[dataValue] [money] NOT NULL,
 CONSTRAINT [PK_DailyTotals] PRIMARY KEY CLUSTERED 
(
	[franchiseMask] ASC,
	[dataType] ASC,
	[dataTime] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DrivingRoutes]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DrivingRoutes](
	[lookupKey] [varchar](200) NOT NULL,
	[distance] [money] NOT NULL,
	[travelTime] [money] NOT NULL,
 CONSTRAINT [PK_DrivingRoutes] PRIMARY KEY CLUSTERED 
(
	[lookupKey] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FollowUp]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FollowUp](
	[appointmentID] [int] NOT NULL,
	[createdOn] [datetime] NOT NULL,
	[schedulingSatisfaction] [int] NOT NULL,
	[timeManagement] [int] NOT NULL,
	[professionalism] [int] NOT NULL,
	[cleaningQuality] [int] NOT NULL,
	[notes] [varchar](max) NOT NULL,
 CONSTRAINT [PK_FollowUp] PRIMARY KEY CLUSTERED 
(
	[appointmentID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Franchise]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Franchise](
	[franchiseID] [int] NOT NULL,
	[franchiseName] [varchar](50) NULL,
	[email] [varchar](75) NULL,
	[emailPassword] [varchar](75) NULL,
	[emailSmtp] [varchar](75) NULL,
	[emailPort] [int] NOT NULL,
	[emailSSL] [bit] NOT NULL,
	[phone] [varchar](15) NULL,
	[defaultRatePerHour] [money] NOT NULL,
	[defaultServiceFee] [money] NOT NULL,
	[defaultScheduleFee] [money] NOT NULL,
	[rewardsPercentage] [money] NOT NULL,
	[carPercentage] [money] NOT NULL,
	[suppliesPercentage] [money] NOT NULL,
	[address] [varchar](200) NULL,
	[city] [varchar](100) NULL,
	[defaultState] [varchar](50) NULL,
	[zip] [varchar](10) NULL,
	[webLink] [varchar](200) NULL,
	[notesEnabled] [bit] NOT NULL,
	[notesGeneral] [varchar](max) NULL,
	[notesAccounting] [varchar](max) NULL,
	[sendSchedules] [datetime] NOT NULL,
	[ePNAccount] [varchar](50) NULL,
	[restrictKey] [varchar](50) NULL,
	[batchTime] [datetime] NOT NULL,
	[advertisementList] [varchar](max) NOT NULL,
	[adjustmentList] [varchar](max) NOT NULL,
	[paymentList] [varchar](max) NOT NULL,
	[partnerCategoryList] [varchar](max) NOT NULL,
	[scheduleFeeString] [varchar](200) NOT NULL,
	[smsUsername] [varchar](2000) NULL,
	[smsPassword] [varchar](300) NULL,
	[salesTax] [money] NOT NULL,
	[reviewUsLink] [varchar](500) NOT NULL,
 CONSTRAINT [PK_Franchise] PRIMARY KEY CLUSTERED 
(
	[franchiseID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GiftCards]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GiftCards](
	[giftCardID] [int] NOT NULL,
	[customerID] [int] NOT NULL,
	[paymentType] [varchar](50) NOT NULL,
	[paymentID] [varchar](50) NULL,
	[lastFourCard] [varchar](16) NULL,
	[batched] [bit] NOT NULL,
	[isVoid] [bit] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[amount] [money] NOT NULL,
	[redeemed] [int] NOT NULL,
	[giverName] [varchar](100) NOT NULL,
	[recipientName] [varchar](100) NOT NULL,
	[recipientEmail] [varchar](100) NOT NULL,
	[billingEmail] [varchar](100) NOT NULL,
	[message] [varchar](max) NOT NULL,
	[username] [varchar](50) NULL,
 CONSTRAINT [PK_GiftCards] PRIMARY KEY CLUSTERED 
(
	[giftCardID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HomeGuardContract]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HomeGuardContract](
	[customerID] [int] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[dateSent] [varchar](50) NULL,
	[dateSigned] [varchar](50) NULL,
	[customerName] [varchar](50) NULL,
	[customerAddress] [varchar](100) NULL,
	[customerPhoneOne] [varchar](50) NULL,
	[customerPhoneTwo] [varchar](50) NULL,
	[customerEmail] [varchar](100) NULL,
	[ipAddress] [varchar](50) NULL,
	[HW_Frequency] [varchar](50) NULL,
	[HW_GarbageCans] [bit] NOT NULL,
	[HW_GarbageDay] [varchar](50) NULL,
	[HW_PlantsWatered] [bit] NOT NULL,
	[HW_PlantsWateredFrequency] [varchar](50) NULL,
	[HW_Thermostat] [bit] NOT NULL,
	[HW_ThermostatTemperature] [varchar](50) NULL,
	[HW_Breakers] [bit] NOT NULL,
	[HW_BreakersLocation] [varchar](50) NULL,
	[HW_CleanBeforeReturn] [bit] NOT NULL,
	[HW_Details] [varchar](max) NULL,
 CONSTRAINT [PK_HomeGaurdContract] PRIMARY KEY CLUSTERED 
(
	[customerID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceRange]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceRange](
	[invoiceID] [int] IDENTITY(1000,1) NOT NULL,
	[customerID] [int] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[startDate] [datetime] NOT NULL,
	[endDate] [datetime] NOT NULL,
	[email] [varchar](500) NOT NULL,
	[memo] [varchar](500) NOT NULL,
 CONSTRAINT [PK_InvoiceRange] PRIMARY KEY CLUSTERED 
(
	[invoiceID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MassEmail]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MassEmail](
	[massEmailID] [int] IDENTITY(1,1) NOT NULL,
	[status] [int] NOT NULL,
	[customerID] [int] NOT NULL,
	[contractorID] [int] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[dateSent] [datetime] NULL,
	[subject] [varchar](200) NOT NULL,
	[body] [varchar](max) NOT NULL,
 CONSTRAINT [PK_MassEmail] PRIMARY KEY CLUSTERED 
(
	[massEmailID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Partners]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Partners](
	[companyName] [varchar](100) NOT NULL,
	[franchiseMask] [int] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[category] [varchar](50) NOT NULL,
	[phoneNumber] [varchar](50) NOT NULL,
	[webAddress] [varchar](100) NOT NULL,
	[description] [varchar](max) NOT NULL,
	[approved] [bit] NOT NULL,
 CONSTRAINT [PK_Partners] PRIMARY KEY CLUSTERED 
(
	[companyName] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuoteTemplate]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuoteTemplate](
	[templateName] [varchar](50) NOT NULL,
	[franchiseMask] [int] NOT NULL,
	[templateValue] [varchar](max) NOT NULL,
 CONSTRAINT [PK_QuoteTemplate_1] PRIMARY KEY CLUSTERED 
(
	[templateName] ASC,
	[franchiseMask] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceRequest]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceRequest](
	[serviceRequestID] [int] IDENTITY(1000,1) NOT NULL,
	[customerID] [int] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[requestDate] [date] NOT NULL,
	[timePrefix] [varchar](50) NOT NULL,
	[timeSuffix] [varchar](50) NOT NULL,
	[notes] [varchar](max) NOT NULL,
 CONSTRAINT [PK_CleaningRequest] PRIMARY KEY CLUSTERED 
(
	[serviceRequestID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[transID] [int] IDENTITY(1,1) NOT NULL,
	[transType] [varchar](50) NOT NULL,
	[paymentID] [varchar](50) NULL,
	[paymentType] [varchar](50) NOT NULL,
	[lastFourCard] [varchar](50) NULL,
	[batched] [bit] NOT NULL,
	[customerID] [int] NULL,
	[dateCreated] [datetime] NOT NULL,
	[dateApply] [datetime] NOT NULL,
	[isVoid] [bit] NOT NULL,
	[total] [money] NOT NULL,
	[hoursBilled] [money] NOT NULL,
	[hourlyRate] [money] NOT NULL,
	[serviceFee] [money] NOT NULL,
	[subContractorCC] [money] NOT NULL,
	[subContractorWW] [money] NOT NULL,
	[subContractorHW] [money] NOT NULL,
	[subContractorCL] [money] NOT NULL,
	[discountAmount] [money] NOT NULL,
	[discountPercent] [money] NOT NULL,
	[tips] [money] NOT NULL,
	[email] [varchar](100) NULL,
	[emailSent] [bit] NOT NULL,
	[notes] [varchar](500) NULL,
	[username] [varchar](100) NULL,
	[discountReferral] [money] NOT NULL,
	[auth] [int] NOT NULL,
	[salesTax] [money] NOT NULL,
 CONSTRAINT [PK_Transaction_1] PRIMARY KEY CLUSTERED 
(
	[transID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Unavailable]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Unavailable](
	[unavailableID] [int] NOT NULL,
	[contractorID] [int] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[dateRequested] [datetime] NOT NULL,
	[startTime] [datetime] NOT NULL,
	[endTime] [datetime] NOT NULL,
	[recurrenceID] [int] NOT NULL,
	[recurrenceType] [int] NOT NULL,
 CONSTRAINT [PK_Unavailable] PRIMARY KEY CLUSTERED 
(
	[unavailableID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[username] [varchar](50) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[access] [int] NOT NULL,
	[franchiseMask] [int] NOT NULL,
	[contractorID] [int] NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[username] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_appStatus]  DEFAULT ((0)) FOR [appStatus]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_appType]  DEFAULT ((1)) FOR [appType]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_dateCreated]  DEFAULT (getutcdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_dateUpddated]  DEFAULT (getutcdate()) FOR [dateUpdated]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_startTime]  DEFAULT ('1900-01-01 9:00') FOR [startTime]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_endTime]  DEFAULT ('1900-01-01 9:00') FOR [endTime]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_customerHours]  DEFAULT ((0)) FOR [customerHours]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_customerRate]  DEFAULT ((0)) FOR [customerRate]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_serviceFee]  DEFAULT ((0)) FOR [customerServiceFee]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_customerSubContractor]  DEFAULT ((0)) FOR [customerSubContractor]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_customerDiscountAmount]  DEFAULT ((0)) FOR [customerDiscountAmount]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_customerDiscountPercent]  DEFAULT ((0)) FOR [customerDiscountPercent]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_contractorHours]  DEFAULT ((0)) FOR [contractorHours]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_contractorRate]  DEFAULT ((0)) FOR [contractorRate]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_contractorTipsGas]  DEFAULT ((0)) FOR [contractorTips]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_contractorAdjustAmount]  DEFAULT ((0)) FOR [contractorAdjustAmount]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_amountPaid]  DEFAULT ((0)) FOR [amountPaid]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_paymentFinished]  DEFAULT ((0)) FOR [paymentFinished]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_recurrenceID]  DEFAULT ((0)) FOR [recurrenceID]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_recurrenceType]  DEFAULT ((0)) FOR [recurrenceType]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_weeklyFrequency]  DEFAULT ((1)) FOR [weeklyFrequency]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_monthlyWeek]  DEFAULT ((0)) FOR [monthlyWeek]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_monthlyDay]  DEFAULT ((0)) FOR [monthlyDay]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_confirmed]  DEFAULT ((0)) FOR [confirmed]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_leftMessage]  DEFAULT ((0)) FOR [leftMessage]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_keysReturned]  DEFAULT ((0)) FOR [keysReturned]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_followUpSent]  DEFAULT ((0)) FOR [followUpSent]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_sentSMS]  DEFAULT ((0)) FOR [sentSMS]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_sentWeekSMS]  DEFAULT ((0)) FOR [sentWeekSMS]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_sentEmail]  DEFAULT ((0)) FOR [sentEmail]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_customerDiscountReferral]  DEFAULT ((0)) FOR [customerDiscountReferral]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_usernameBooked]  DEFAULT ((0)) FOR [usernameBooked]
GO
ALTER TABLE [dbo].[Appointments] ADD  CONSTRAINT [DF_Appointments_salesTax]  DEFAULT ((0)) FOR [salesTax]
GO
ALTER TABLE [dbo].[CleaningPacks] ADD  CONSTRAINT [DF_CleaningPacks_batched]  DEFAULT ((0)) FOR [batched]
GO
ALTER TABLE [dbo].[CleaningPacks] ADD  CONSTRAINT [DF_CleaningPacks_isVoid]  DEFAULT ((0)) FOR [isVoid]
GO
ALTER TABLE [dbo].[CleaningPacks] ADD  CONSTRAINT [DF_CleaningPacks_dateCreated]  DEFAULT (getutcdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_contractorType]  DEFAULT ((1)) FOR [contractorType]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_dateCreated]  DEFAULT (getutcdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_paymentDay]  DEFAULT ('Tuesday') FOR [paymentDay]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_hourlyRate]  DEFAULT ((0)) FOR [hourlyRate]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_serviceSplit]  DEFAULT ((0)) FOR [serviceSplit]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_hireDate]  DEFAULT ('1/1/1900') FOR [hireDate]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_accountRep]  DEFAULT ((0)) FOR [accountRep]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_scheduled]  DEFAULT ((0)) FOR [scheduled]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_sendSchedules]  DEFAULT ((0)) FOR [sendSchedules]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_lastSchedule]  DEFAULT ('1-1-1900') FOR [lastSchedule]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_sendPayroll]  DEFAULT ((0)) FOR [sendPayroll]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_lastPayroll]  DEFAULT ('1-1-1900') FOR [lastPayroll]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_team]  DEFAULT (space((0))) FOR [team]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_score]  DEFAULT ((0)) FOR [score]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_startDay]  DEFAULT ('9:00 AM') FOR [startDay]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_endDay]  DEFAULT ('5:00 PM') FOR [endDay]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_birthday]  DEFAULT ('1-1-1900') FOR [birthday]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_waiverDate]  DEFAULT ('1-1-1900') FOR [waiverDate]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_waiverUpdateDate]  DEFAULT ('1-1-1900') FOR [waiverUpdateDate]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_insuranceDate]  DEFAULT ('1-1-1900') FOR [insuranceDate]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_insuranceUpdateDate]  DEFAULT ('1-1-1900') FOR [insuranceUpdateDate]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_backgroundCheck]  DEFAULT ('1-1-1900') FOR [backgroundCheck]
GO
ALTER TABLE [dbo].[Contractors] ADD  CONSTRAINT [DF_Contractors_applicant]  DEFAULT ((0)) FOR [applicant]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_bookedDate]  DEFAULT (getutcdate()) FOR [bookedDate]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_updateDate]  DEFAULT (getutcdate()) FOR [lastUpdate]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_newBuilding]  DEFAULT ((0)) FOR [newBuilding]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_sectionMask]  DEFAULT ((1)) FOR [sectionMask]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_billingSame]  DEFAULT ((1)) FOR [billingSame]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_serviceFee]  DEFAULT ((5)) FOR [serviceFee]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_ratePerHour]  DEFAULT ((25)) FOR [ratePerHour]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_discountPercent]  DEFAULT ((0)) FOR [discountPercent]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_discountAmount]  DEFAULT ((0)) FOR [discountAmount]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_estimatedHours]  DEFAULT ((0)) FOR [estimatedHours]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_NC_Vacuum]  DEFAULT ((0)) FOR [NC_Vacuum]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_NC_DoDishes]  DEFAULT ((0)) FOR [NC_DoDishes]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_NC_ChangeBed]  DEFAULT ((0)) FOR [NC_ChangeBed]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_NC_FlooringCarpet]  DEFAULT ((0)) FOR [NC_FlooringCarpet]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_NC_FlooringHardwood]  DEFAULT ((0)) FOR [NC_FlooringHardwood]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_NC_FlooringTile]  DEFAULT ((0)) FOR [NC_FlooringTile]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_NC_FlooringLinoleum]  DEFAULT ((0)) FOR [NC_FlooringLinoleum]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_NC_FlooringSlate]  DEFAULT ((0)) FOR [NC_FlooringSlate]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_NC_FlooringMarble]  DEFAULT ((0)) FOR [NC_FlooringMarble]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_NC_RequiresKeys]  DEFAULT ((0)) FOR [NC_RequiresKeys]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_NC_Organize]  DEFAULT ((0)) FOR [NC_Organize]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_Blinds]  DEFAULT ((0)) FOR [DC_Blinds]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_Windows]  DEFAULT ((0)) FOR [DC_Windows]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_WindowsSills]  DEFAULT ((0)) FOR [DC_WindowsSills]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_Walls]  DEFAULT ((0)) FOR [DC_Walls]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_Baseboards]  DEFAULT ((0)) FOR [DC_Baseboards]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_DoorFrames]  DEFAULT ((0)) FOR [DC_DoorFrames]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_CeilingFans]  DEFAULT ((0)) FOR [DC_CeilingFans]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_LightFixtures]  DEFAULT ((0)) FOR [DC_LightFixtures]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_KitchenCuboards]  DEFAULT ((0)) FOR [DC_KitchenCuboards]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_BathroomCuboards]  DEFAULT ((0)) FOR [DC_BathroomCuboards]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_Oven]  DEFAULT ((0)) FOR [DC_Oven]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_Refigerator]  DEFAULT ((0)) FOR [DC_Refrigerator]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_welcomeLetter]  DEFAULT ((0)) FOR [welcomeLetter]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_quoteReply]  DEFAULT ((0)) FOR [quoteReply]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_LightSwitches]  DEFAULT ((0)) FOR [DC_LightSwitches]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_VentCovers]  DEFAULT ((0)) FOR [DC_VentCovers]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_points]  DEFAULT ((0)) FOR [points]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_rewardsEnabled]  DEFAULT ((0)) FOR [rewardsEnabled]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_referredBy]  DEFAULT ((0)) FOR [referredBy]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_bestPhoneCell]  DEFAULT ((0)) FOR [bestPhoneCell]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_alternatePhoneOneCell]  DEFAULT ((0)) FOR [alternatePhoneOneCell]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_alternatePhoneTwoCell]  DEFAULT ((0)) FOR [alternatePhoneTwoCell]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_sendPromotions]  DEFAULT ((1)) FOR [sendPromotions]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_NC_RequestEcoCleaners]  DEFAULT ((0)) FOR [NC_RequestEcoCleaners]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_InsideVents]  DEFAULT ((0)) FOR [DC_InsideVents]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_Pantry]  DEFAULT ((0)) FOR [DC_Pantry]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DC_LaundryRoom]  DEFAULT ((0)) FOR [DC_LaundryRoom]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_loginInfoSent]  DEFAULT ((0)) FOR [loginInfoSent]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_CC_PetOdorAdditive]  DEFAULT ((0)) FOR [CC_PetOdorAdditive]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_WW_VaultedCeilings]  DEFAULT ((0)) FOR [WW_VaultedCeilings]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_WW_PostConstruction]  DEFAULT ((0)) FOR [WW_PostConstruction]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_WW_Razor]  DEFAULT ((0)) FOR [WW_Razor]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_WW_HardWater]  DEFAULT ((0)) FOR [WW_HardWater]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_WW_FrenchWindows]  DEFAULT ((0)) FOR [WW_FrenchWindows]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_WW_StormWindows]  DEFAULT ((0)) FOR [WW_StormWindows]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_WW_Screens]  DEFAULT ((0)) FOR [WW_Screens]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_WW_Tracks]  DEFAULT ((0)) FOR [WW_Tracks]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_WW_Wells]  DEFAULT ((0)) FOR [WW_Wells]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_WW_Gutters]  DEFAULT ((0)) FOR [WW_Gutters]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_HW_GarbageCans]  DEFAULT ((0)) FOR [HW_GarbageCans]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_HW_PlantsWatered]  DEFAULT ((0)) FOR [HW_PlantsWatered]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_HW_Thermostat]  DEFAULT ((0)) FOR [HW_Thermostat]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_HW_Breakers]  DEFAULT ((0)) FOR [HW_Breakers]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_HW_CleanBeforeReturn]  DEFAULT ((0)) FOR [HW_CleanBeforeReturn]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_reviewUsDate]  DEFAULT ('1/1/1900') FOR [reviewUsDate]
GO
ALTER TABLE [dbo].[FollowUp] ADD  CONSTRAINT [DF_FollowUp_createdOn]  DEFAULT (getutcdate()) FOR [createdOn]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_emailPort]  DEFAULT ((25)) FOR [emailPort]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_emailSSL]  DEFAULT ((0)) FOR [emailSSL]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_defaultRatePerHour]  DEFAULT ((0)) FOR [defaultRatePerHour]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_defaultServiceFee]  DEFAULT ((0)) FOR [defaultServiceFee]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_defaultScheduleFee]  DEFAULT ((0)) FOR [defaultScheduleFee]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_rewardsPercentage]  DEFAULT ((0)) FOR [rewardsPercentage]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_carPercent]  DEFAULT ((0)) FOR [carPercentage]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_suppliesPercent]  DEFAULT ((0)) FOR [suppliesPercentage]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_notesEnabled]  DEFAULT ((0)) FOR [notesEnabled]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_sendSchedules]  DEFAULT ('1/1/1900 20:00') FOR [sendSchedules]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_batchTime]  DEFAULT ('1-1-1900') FOR [batchTime]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_advertisementList_1]  DEFAULT ('Google Search|Google Maps|Google Top/Side|Angies List|Word Of Mouth|Yellow Pages|Unknown|Other') FOR [advertisementList]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_adjustmentList]  DEFAULT ('None|Aflac|Supplies|Advance|Breakage|Redo|Lawyer|Child Support|Theft|No Show|Other') FOR [adjustmentList]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_paymentList]  DEFAULT ('Credit Card|Visa|Master Card|Discover|Check|Check (In Mail)|Cash|Gift Certificate|Trade|Invoice (print)|Need CC Info') FOR [paymentList]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_partnerCategoryList]  DEFAULT ('Carpet Cleaning|Window Cleaning|Accountants|Electricians|Moving Companies|Pet Care|Emergency Restoration|Chiropractic|Realtors|Plumbing|Printing|Other') FOR [partnerCategoryList]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_scheduleFeeString]  DEFAULT ((4)) FOR [scheduleFeeString]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_salesTax]  DEFAULT ((0)) FOR [salesTax]
GO
ALTER TABLE [dbo].[Franchise] ADD  CONSTRAINT [DF_Franchise_reviewUsLink]  DEFAULT ('') FOR [reviewUsLink]
GO
ALTER TABLE [dbo].[GiftCards] ADD  CONSTRAINT [DF_GiftCards_batched]  DEFAULT ((0)) FOR [batched]
GO
ALTER TABLE [dbo].[GiftCards] ADD  CONSTRAINT [DF_GiftCards_isVoid]  DEFAULT ((0)) FOR [isVoid]
GO
ALTER TABLE [dbo].[GiftCards] ADD  CONSTRAINT [DF_GiftCards_dateCreated]  DEFAULT (getutcdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[GiftCards] ADD  CONSTRAINT [DF_GiftCards_redeemed]  DEFAULT ((0)) FOR [redeemed]
GO
ALTER TABLE [dbo].[HomeGuardContract] ADD  CONSTRAINT [DF_HomeGaurdContract_dateCreated]  DEFAULT (getutcdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[HomeGuardContract] ADD  CONSTRAINT [DF_HomeGaurdContract_HW_GarbageCans]  DEFAULT ((0)) FOR [HW_GarbageCans]
GO
ALTER TABLE [dbo].[HomeGuardContract] ADD  CONSTRAINT [DF_HomeGaurdContract_HW_PlantsWatered]  DEFAULT ((0)) FOR [HW_PlantsWatered]
GO
ALTER TABLE [dbo].[HomeGuardContract] ADD  CONSTRAINT [DF_HomeGaurdContract_HW_Thermostat]  DEFAULT ((0)) FOR [HW_Thermostat]
GO
ALTER TABLE [dbo].[HomeGuardContract] ADD  CONSTRAINT [DF_HomeGuardContract_HW_Breakers]  DEFAULT ((0)) FOR [HW_Breakers]
GO
ALTER TABLE [dbo].[HomeGuardContract] ADD  CONSTRAINT [DF_HomeGaurdContract_HW_CleanBeforeReturn]  DEFAULT ((0)) FOR [HW_CleanBeforeReturn]
GO
ALTER TABLE [dbo].[InvoiceRange] ADD  CONSTRAINT [DF_InvoiceRange_dateCreated]  DEFAULT (getutcdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[InvoiceRange] ADD  CONSTRAINT [DF_InvoiceRange_email]  DEFAULT ('') FOR [email]
GO
ALTER TABLE [dbo].[InvoiceRange] ADD  CONSTRAINT [DF_InvoiceRange_memo]  DEFAULT ('') FOR [memo]
GO
ALTER TABLE [dbo].[MassEmail] ADD  CONSTRAINT [DF_MassEmail_status]  DEFAULT ((0)) FOR [status]
GO
ALTER TABLE [dbo].[MassEmail] ADD  CONSTRAINT [DF_MassEmail_customerID]  DEFAULT ((0)) FOR [customerID]
GO
ALTER TABLE [dbo].[MassEmail] ADD  CONSTRAINT [DF_MassEmail_contractorID]  DEFAULT ((0)) FOR [contractorID]
GO
ALTER TABLE [dbo].[MassEmail] ADD  CONSTRAINT [DF_MassEmail_dateCreated]  DEFAULT (getutcdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[Partners] ADD  CONSTRAINT [DF_Partners_franchiseMask]  DEFAULT ((0)) FOR [franchiseMask]
GO
ALTER TABLE [dbo].[Partners] ADD  CONSTRAINT [DF_Partners_dateCreated]  DEFAULT (getutcdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[Partners] ADD  CONSTRAINT [DF_Partners_approved]  DEFAULT ((0)) FOR [approved]
GO
ALTER TABLE [dbo].[ServiceRequest] ADD  CONSTRAINT [DF_CleaningRequest_dateCreated]  DEFAULT (getutcdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_batched]  DEFAULT ((0)) FOR [batched]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_dateCreated]  DEFAULT (getutcdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_dateApply]  DEFAULT (getutcdate()) FOR [dateApply]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_void]  DEFAULT ((0)) FOR [isVoid]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_hoursAmount]  DEFAULT ((0)) FOR [total]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_amount]  DEFAULT ((0)) FOR [hoursBilled]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_hoursCount]  DEFAULT ((0)) FOR [hourlyRate]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_serviceFeeAmount]  DEFAULT ((0)) FOR [serviceFee]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_subContractorCC]  DEFAULT ((0)) FOR [subContractorCC]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_subContractorWW]  DEFAULT ((0)) FOR [subContractorWW]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_subContractorHW]  DEFAULT ((0)) FOR [subContractorHW]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_subContractorCL]  DEFAULT ((0)) FOR [subContractorCL]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_discount]  DEFAULT ((0)) FOR [discountAmount]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_discountPercent]  DEFAULT ((0)) FOR [discountPercent]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_tipsAmount]  DEFAULT ((0)) FOR [tips]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_emailSent]  DEFAULT ((0)) FOR [emailSent]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_discountReferral]  DEFAULT ((0)) FOR [discountReferral]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_auth_1]  DEFAULT ((0)) FOR [auth]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_salesTax]  DEFAULT ((0)) FOR [salesTax]
GO
ALTER TABLE [dbo].[Unavailable] ADD  CONSTRAINT [DF_Unavailable_dateCreated]  DEFAULT (getutcdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[Unavailable] ADD  CONSTRAINT [DF_Unavailable_startTime]  DEFAULT ('1900-01-01 9:00') FOR [startTime]
GO
ALTER TABLE [dbo].[Unavailable] ADD  CONSTRAINT [DF_Unavailable_endTime]  DEFAULT ('1900-01-01 9:00') FOR [endTime]
GO
ALTER TABLE [dbo].[Unavailable] ADD  CONSTRAINT [DF_Unavailable_recurrenceID]  DEFAULT ((0)) FOR [recurrenceID]
GO
ALTER TABLE [dbo].[Unavailable] ADD  CONSTRAINT [DF_Unavailable_recurrenceType]  DEFAULT ((0)) FOR [recurrenceType]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Accounts_access]  DEFAULT ((0)) FOR [access]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Accounts_franchise]  DEFAULT ((0)) FOR [franchiseMask]
GO
ALTER TABLE [dbo].[Appointments]  WITH NOCHECK ADD  CONSTRAINT [FK_Appointments_Contractors] FOREIGN KEY([contractorID])
REFERENCES [dbo].[Contractors] ([contractorID])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Contractors]
GO
ALTER TABLE [dbo].[Appointments]  WITH NOCHECK ADD  CONSTRAINT [FK_Appointments_Customers] FOREIGN KEY([customerID])
REFERENCES [dbo].[Customers] ([customerID])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointments_Customers]
GO
ALTER TABLE [dbo].[Customers]  WITH NOCHECK ADD  CONSTRAINT [FK_Customers_Contractors] FOREIGN KEY([bookedBy])
REFERENCES [dbo].[Contractors] ([contractorID])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_Customers_Contractors]
GO
ALTER TABLE [dbo].[FollowUp]  WITH NOCHECK ADD  CONSTRAINT [FK_FollowUp_Appointments] FOREIGN KEY([appointmentID])
REFERENCES [dbo].[Appointments] ([appointmentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FollowUp] CHECK CONSTRAINT [FK_FollowUp_Appointments]
GO
ALTER TABLE [dbo].[GiftCards]  WITH NOCHECK ADD  CONSTRAINT [FK_GiftCards_Customers] FOREIGN KEY([customerID])
REFERENCES [dbo].[Customers] ([customerID])
GO
ALTER TABLE [dbo].[GiftCards] CHECK CONSTRAINT [FK_GiftCards_Customers]
GO
ALTER TABLE [dbo].[InvoiceRange]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceRange_Customers] FOREIGN KEY([customerID])
REFERENCES [dbo].[Customers] ([customerID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceRange] CHECK CONSTRAINT [FK_InvoiceRange_Customers]
GO
ALTER TABLE [dbo].[Transaction]  WITH NOCHECK ADD  CONSTRAINT [FK_Transaction_Customers] FOREIGN KEY([customerID])
REFERENCES [dbo].[Customers] ([customerID])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Customers]
GO
ALTER TABLE [dbo].[Unavailable]  WITH NOCHECK ADD  CONSTRAINT [FK_Unavailable_Contractors] FOREIGN KEY([contractorID])
REFERENCES [dbo].[Contractors] ([contractorID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Unavailable] CHECK CONSTRAINT [FK_Unavailable_Contractors]
GO
ALTER TABLE [dbo].[Users]  WITH NOCHECK ADD  CONSTRAINT [FK_Users_Contractors] FOREIGN KEY([contractorID])
REFERENCES [dbo].[Contractors] ([contractorID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Contractors]
GO
/****** Object:  StoredProcedure [dbo].[CreateRecurringApps]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateRecurringApps] 

@recurrenceID int

AS
BEGIN

DECLARE @appointmentID int;
DECLARE @appointmentDate datetime;
DECLARE @appType int;
DECLARE @startTime datetime;
DECLARE @endTime datetime;
DECLARE @customerID int;
DECLARE @customerRate money;
DECLARE @customerServiceFee money;
DECLARE @customerSubContractor money;
DECLARE @contractorID int;
DECLARE @recurrenceType int;
DECLARE @weeklyFrequency int;
DECLARE @monthlyWeek int;
DECLARE @monthlyDay int;

DECLARE @currDate datetime;
DECLARE @maxDate datetime;
DECLARE @appHours money;
--DECLARE @appCount int;
DECLARE @maxCount int;
DECLARE @newAppID int;
DECLARE @contractorRate money;
DECLARE @salesTax money;

IF @recurrenceID IS NOT NULL AND @recurrenceID > 0
BEGIN
	SELECT TOP 1 
		@appointmentID = appointmentID,
		@appointmentDate = appointmentDate,
		@appType = appType,
		@startTime = startTime,
		@endTime = endTime,
		@customerID = customerID,
		@customerRate = customerRate,
		@customerServiceFee = customerServiceFee,
		@customerSubContractor = customerSubContractor,
		@contractorID = contractorID,
		@recurrenceType = recurrenceType,
		@weeklyFrequency = weeklyFrequency,
		@monthlyWeek = monthlyWeek,
		@monthlyDay = monthlyDay,
		@salesTax = salesTax
	FROM 
		Appointments 
	WHERE 
		recurrenceID = @recurrenceID 
	ORDER BY 
		appointmentDate DESC;
		
	IF @appointmentID IS NOT NULL AND @recurrenceType > 0
	BEGIN
		SET DATEFIRST 1;
		SET @currDate = DATEADD(HOUR, -7, GETUTCDATE());
		SET @maxDate = DATEADD(DAY, 90, @currDate);
		SET @appHours = DATEDIFF(MINUTE, @startTime, @endTime) / 60.0;
		IF @weeklyFrequency <= 0 SET @weeklyFrequency = 1;
		--SET @appCount = (SELECT COUNT(*) FROM Appointments WHERE recurrenceID = @recurrenceID AND appointmentDate > @currDate);
		SET @maxCount = 0;

		WHILE @appointmentDate < @maxDate AND @maxCount < 150
		BEGIN
			IF @recurrenceType = 2 SET @appointmentDate = dbo.DayInMonth(@appointmentDate,@monthlyDay,@monthlyWeek);
			ELSE SET @appointmentDate = DATEADD(DAY, @weeklyFrequency * 7, @appointmentDate);
			
			IF @appointmentDate >= @currDate
			BEGIN
				SET @newAppID = dbo.InlineMax((SELECT MAX(appointmentID) + 1 FROM Appointments), 10000000);
				SET @contractorRate = (CASE WHEN @contractorID IS NULL THEN 0 ELSE (SELECT hourlyRate FROM Contractors WHERE contractorID = @contractorID) END);
			
				INSERT INTO Appointments
					(appointmentID,
					appointmentDate,
					appType,
					startTime,
					endTime,
					customerID,
					customerHours,
					customerRate,
					customerServiceFee,
					customerSubContractor,
					contractorID,
					contractorHours,
					contractorRate,
					recurrenceID,
					recurrenceType,
					weeklyFrequency,
					monthlyWeek,
					monthlyDay,
					salesTax)
				VALUES
					(@newAppID,
					@appointmentDate,
					@appType,
					@startTime,
					@endTime,
					@customerID,
					@appHours,
					@customerRate,
					@customerServiceFee,
					@customerSubContractor,
					@contractorID,
					@appHours,
					@contractorRate,
					@recurrenceID,
					@recurrenceType,
					@weeklyFrequency,
					@monthlyWeek,
					@monthlyDay,
					@salesTax);
			END
				
			--SET @appCount = (SELECT COUNT(*) FROM Appointments WHERE recurrenceID = @recurrenceID AND appointmentDate > @currDate);
			SET @maxCount = @maxCount + 1;
		END
	END
END
	
END
GO
/****** Object:  StoredProcedure [dbo].[CreateRecurringUnavailable]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateRecurringUnavailable] 

@recurrenceID int

AS
BEGIN

DECLARE @unavailableID int;
DECLARE @contractorID int;
DECLARE @dateRequested datetime;
DECLARE @startTime datetime;
DECLARE @endTime datetime;
DECLARE @recurrenceType int;

DECLARE @currDate datetime;
DECLARE @maxDate datetime;
DECLARE @iterationCount int;
DECLARE @maxCount int;
DECLARE @newUnID int;
DECLARE @contractorRate money;

IF @recurrenceID IS NOT NULL AND @recurrenceID > 0
BEGIN
	SELECT TOP 1 
		@unavailableID = unavailableID,
		@contractorID = contractorID,
		@dateRequested = dateRequested,
		@startTime = startTime,
		@endTime = endTime,
		@recurrenceType = recurrenceType
	FROM 
		Unavailable
	WHERE 
		recurrenceID = @recurrenceID 
	ORDER BY 
		dateRequested DESC;
		
	IF @unavailableID IS NOT NULL AND @recurrenceType > 0
	BEGIN
		SET DATEFIRST 1;
		SET @currDate = DATEADD(HOUR, -7, GETUTCDATE());
		SET @maxDate = DATEADD(DAY, 90, @currDate);
		SET @maxCount = 0;
		
		SET @iterationCount = 21;
		IF @recurrenceType = 2 SET @iterationCount = 3;

		WHILE @dateRequested < @maxDate AND @maxCount < 150
		BEGIN	
			SET @dateRequested = DATEADD(DAY, 1, @dateRequested);
			IF @recurrenceType = 2 SET @dateRequested = DATEADD(DAY, 6, @dateRequested);
			IF @recurrenceType = 3 SET @dateRequested = DATEADD(DAY, 13, @dateRequested);
			
			IF @dateRequested >= @currDate
			BEGIN
				SET @newUnID = (SELECT MAX(unavailableID) + 1 FROM Unavailable);
				
				INSERT INTO Unavailable
					(unavailableID,
					contractorID,
					dateRequested,
					startTime,
					endTime,
					recurrenceID,
					recurrenceType)
				VALUES
					(@newUnID,
					@contractorID,
					@dateRequested,
					@startTime,
					@endTime,
					@recurrenceID,
					@recurrenceType);
			END

			SET @maxCount = @maxCount + 1;
		END
	END
END
	
END
GO
/****** Object:  StoredProcedure [dbo].[SplitServiceFee]    Script Date: 6/5/2024 3:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SplitServiceFee] 

@custID int,
@appDate datetime

AS
BEGIN

DECLARE @appCount int;
DECLARE @serviceFee money;

SELECT
	@appCount = count(*)
FROM
	Appointments
WHERE
	appointmentDate = @appDate AND
	customerID = @custID;

SELECT
	@serviceFee = serviceFee
FROM
	Customers
WHERE
	customerID = @custID;

IF @appCount > 0 AND @serviceFee > 0
BEGIN
	UPDATE
		Appointments
	SET
		customerServiceFee = (@serviceFee / @appCount)
	WHERE
		appointmentDate = @appDate AND
		customerID = @custID;
END
	
END
GO
