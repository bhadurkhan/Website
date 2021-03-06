USE [Garbage]
GO
/****** Object:  UserDefinedTableType [dbo].[RequestDetailType]    Script Date: 06/30/2020 6:16:08 PM ******/
CREATE TYPE [dbo].[RequestDetailType] AS TABLE(
	[Time] [time](7) NULL,
	[Date] [date] NULL
)
GO
/****** Object:  StoredProcedure [dbo].[Garbage]    Script Date: 06/30/2020 6:16:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Garbage]
@Action nvarchar(100)=null,
@UserId bigint=null,
@Status bigint=null,
@BloodBankNameId bigint=null,
@Email nvarchar(100)=null,
@Password nvarchar(100)=null,
@Name nvarchar(100)=null,
@ContactNo nvarchar(100)=null,
@RoleId int=null,
@Address  nvarchar(200)=null,
@AvailableStatus  int=null,
@BloodGroup  nvarchar(100)=null,
@Lat  nvarchar(100)=null,
@Long nvarchar(100)=null,

@BloodBankId  bigint=null,
@BloodBankStatus  bigint=null,
@EstimatedRate  money=null,
@BloodQuantity  nvarchar(100)=null,
@Detail  nvarchar(500)=null,
@DonationDate  date=null,
@DateFrom  date=null,
@DateTo  date=null,
@TimeFrom  time(7)=null,
@TimeTo  time(7)=null,
@CreatedDate  date=null,
@NewPassword  nvarchar(50)=null, 

@BloodDonatedToId  bigint=null,
@DonorId  bigint=null,
@BloodDonatedToStatus  bigint=null,
@BloodBankName  nvarchar(200)=null,
@BloodBankNameAdress  nvarchar(200)=null,
@BloodBankNameLat  nvarchar(200)=null,
@BloodBankNameLong  nvarchar(200)=null,

@SendTo  bigint=null,
@AuctionId  bigint=null,
@Sendby  bigint=null,
@ChatComment  nvarchar(200)=null,
@FeedBack  nvarchar(200)=null,

@BloodBankNameStatus  bigint=null,
@ComplaintTitle  nvarchar(200)=null,
@ComplaintId  bigint=null,
@RequestId  bigint=null,
@TotalAmount  money=null,
@AdminComment  nvarchar(200)=null,
@RateId  bigint=null,
@Rate  money=null,
@BidingAmount  money=null,
@Title  nvarchar(200)=null,
@GarbageType  nvarchar(200)=null,
@ComplaintDetail  nvarchar(500)=null,
@Message  nvarchar(500)=null,
@RequestDetailType dbo.RequestDetailType READONLY
as
BEGIN

--Check AUCTION
BEGIN
			Declare @CheckAuctionDateTimeNew bigint=(Select Top(1) AuctionId from Garbage_TblAuction where [Status]=1 AND  Convert(date,DateTo)<Convert(date,GETDATE()))
			if(@CheckAuctionDateTimeNew is not null)
			Begin
				Declare @GetCompanyIdNew bigint=(Select Top(1) CompanyId from Garbage_TblCompanyBiding where AuctionId=@CheckAuctionDateTimeNew Order By BidingAmount DESC)
				Update Garbage_TblAuction set [Status]=0,AssignTo=@GetCompanyIdNew where AuctionId=@CheckAuctionDateTimeNew
			END
			Else
			Begin
				Set @CheckAuctionDateTimeNew =(Select Top(1) AuctionId from Garbage_TblAuction where [Status]=1 
				AND  Convert(date,DateTo)<=Convert(date,GETDATE()) AND CONVERT(time,TimeTo)<=Convert(time,GETDATE()))
				if(@CheckAuctionDateTimeNew is not null)
				begin
					Declare @GetCompanyId1New bigint=(Select Top(1) CompanyId from Garbage_TblCompanyBiding where AuctionId=@CheckAuctionDateTimeNew Order By BidingAmount DESC)
					Update Garbage_TblAuction set [Status]=0,AssignTo=@GetCompanyId1New where AuctionId=@CheckAuctionDateTimeNew
				end
			End
END
--Common---
Begin
if(@Action='Login')
		begin	
			Select Garbage_TblUser.*,Garbage_TblRoles.RoleId as UserRoleId, Garbage_TblRoles.RoleName from Garbage_TblUser
				inner join Garbage_TblRoles on Garbage_TblUser.RoleId=Garbage_TblRoles.RoleId
			 where Garbage_TblUser.Email= @Email AND Garbage_TblUser.[Password]=@Password AND Garbage_TblUser.[Status]=1
		end
if(@Action='Registration')
	Begin
		declare @UserIdChecking bigint= null
		set @UserIdChecking=(Select Top(1) UserId from Garbage_TblUser where Email=@Email)
		if(@UserIdChecking is null)
		begin
			Insert into Garbage_TblUser(Name,Email,[Password],[Status],RoleId,ContactNo,Available)
						Values(@Name,@Email,@Password,1,@RoleId,@ContactNo,1)
		end

	End
if(@Action='AllTypeofUserProfile')
		begin	
			Select Garbage_TblUser.*,Garbage_TblRoles.RoleId as UserRoleId, Garbage_TblRoles.RoleName from Garbage_TblUser
				inner join Garbage_TblRoles on Garbage_TblUser.RoleId=Garbage_TblRoles.RoleId
			 where Garbage_TblUser.UserId=@UserId
		end
if(@Action='UpdateProfile')
	Begin
		
			Update Garbage_TblUser set Name=@Name,ContactNo=@ContactNo where UserId=@UserId
		end
if(@Action='ChangePasswordByAllTypeOfUser')
		begin	
			Update Garbage_TblUser set [Password]=@NewPassword where UserId=@UserId AND [Password]=@Password
		end
End

--Admin
BEGIN
if(@Action='ListofRateForManger')
		begin	
			Select Garbage_TblRates.* from Garbage_TblRates
		end
if(@Action='InsertRateByManger')
		begin
		Set @RateId=(Select Top(1)RateId from Garbage_TblRates where Title=@Title )	
		if(@RateId is null)
		begin
			Insert into Garbage_TblRates(Title,Rate,CreatedOn)values(@Title,@Rate,GETDATE())
		end
		end
if(@Action='UpdateRateByManger')
		begin
		Set @RequestId=(Select Top(1)RateId from Garbage_TblRates where Title=@Title AND RateId!=@RateId)	
		if(@RequestId is null)
		begin
			Update Garbage_TblRates set Title=@Title,Rate=@Rate where RateId=@RateId
		end
		end
if(@Action='ListofEmployeeForManager')
		begin	
			Select Garbage_TblUser.*,Garbage_TblRoles.RoleId as UserRoleId, Garbage_TblRoles.RoleName from Garbage_TblUser
			inner join Garbage_TblRoles on Garbage_TblUser.RoleId=Garbage_TblRoles.RoleId where Garbage_TblRoles.RoleId=2
		end
if(@Action='UpdateEmployeeByManger')
	Begin
			Update Garbage_TblUser set Name=@Name,ContactNo=@ContactNo,Email=@Email where UserId=@UserId
		end
if(@Action='ChangeStatusofAllTypeofUserByManager')
		begin
		Set @Status=(Select Top(1) [Status] from Garbage_TblUser where UserId=@UserId)
		if(@Status=1)
			Begin	
				Update Garbage_TblUser set [Status]=0 where UserId=@UserId
			End
		Else
			Begin
				Update Garbage_TblUser set [Status]=1 where UserId=@UserId
			End
		end
if(@Action='ListofClientForManager')
		begin	
			Select Garbage_TblUser.*,Garbage_TblRoles.RoleId as UserRoleId, Garbage_TblRoles.RoleName from Garbage_TblUser
			inner join Garbage_TblRoles on Garbage_TblUser.RoleId=Garbage_TblRoles.RoleId where Garbage_TblRoles.RoleId=3
		end
if(@Action='ListofCompanyForManager')
		begin	
			Select Garbage_TblUser.*,Garbage_TblRoles.RoleId as UserRoleId, Garbage_TblRoles.RoleName from Garbage_TblUser
			inner join Garbage_TblRoles on Garbage_TblUser.RoleId=Garbage_TblRoles.RoleId where Garbage_TblRoles.RoleId=4
		end
if(@Action='ListOfPendingRequestForAdmin')
		begin	
			Select Garbage_TblRequest.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo,E.Name as EmployeeName
			from Garbage_TblRequest
			inner join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblRequest.UserId
			Left Join Garbage_TblUser E on E.UserId=Garbage_TblRequest.EmployeeId
			where Garbage_TblRequest.[Status]=1

		end
if(@Action='ListOfAllRequestForAdmin')
		begin	
			Select Garbage_TblRequest.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo,E.Name as EmployeeName
			from Garbage_TblRequest
			inner join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblRequest.UserId
			Left Join Garbage_TblUser E on E.UserId=Garbage_TblRequest.EmployeeId
		end
if(@Action='ListOfRequestDetailForAdmin')
		begin	
			Select Garbage_TblRequestDetail.[Date],Garbage_TblRequestDetail.RequestDetailId,Garbage_TblRequestDetail.RequestDetailStatus,Garbage_TblRequestDetail.[Time],
			 Garbage_TblRequest.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo,E.Name as EmployeeName
			from Garbage_TblRequestDetail
			inner join Garbage_TblRequest on Garbage_TblRequestDetail.RequestId=Garbage_TblRequest.RequestId
			inner join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblRequest.UserId
			Left Join Garbage_TblUser E on E.UserId=Garbage_TblRequest.EmployeeId
			where Garbage_TblRequestDetail.RequestId=@RequestId

		end
if(@Action='ListofAuctionForManger')
		begin	
			Select Garbage_TblAuction.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo from Garbage_TblAuction 
			left join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblAuction.AssignTo 
			Order By Garbage_TblAuction.[Status] DESC
		end
if(@Action='InsertAuctionByManger')
		begin
			Insert into Garbage_TblAuction([Address],CreatedDate,DateFrom,DateTo,Detail,EstimatedRate,[Status],TimeFrom,TimeTo)
			values(@Address,GETDATE(),@DateFrom,@DateTo,@Detail,@EstimatedRate,1,@TimeFrom,@TimeTo)
		end
if(@Action='UpdateAuctionByManger')
		begin
			Update Garbage_TblAuction Set [Address]=@Address,DateFrom=@DateFrom,DateTo=@DateTo,Detail=@Detail,EstimatedRate=@EstimatedRate,
			TimeFrom=@TimeFrom,TimeTo=@TimeTo where AuctionId=@AuctionId
		end
if(@Action='ChangeAuctionStatusByManger')
		begin
			Set @Status=(Select Top(1) Status from Garbage_TblAuction where AuctionId=@AuctionId)
			if(@Status=1)
			begin
			Update Garbage_TblAuction set [Status]=0 where AuctionId=@AuctionId
			end
			else
			begin
			Update Garbage_TblAuction set [Status]=1 where AuctionId=@AuctionId
			end
		end
if(@Action='AuctionBiddingDetailForManger')
		begin	
			Select Garbage_TblCompanyBiding.BidingAmount,Garbage_TblCompanyBiding.BidingId,Garbage_TblCompanyBiding.CompanyId,
			Garbage_TblCompanyBiding.CreatedDate as BiddingCreatedDate, C.Name as CompanyName,C.ContactNo as BiddingContactNo,
			Garbage_TblAuction.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo from Garbage_TblCompanyBiding
			left join  Garbage_TblAuction on Garbage_TblAuction.AuctionId=Garbage_TblCompanyBiding.AuctionId
			left join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblAuction.AssignTo 
			left join Garbage_TblUser C on C.UserId=Garbage_TblCompanyBiding.CompanyId 
			where Garbage_TblAuction.AuctionId=@AuctionId Order by Garbage_TblCompanyBiding.BidingAmount DESC
		end
if(@Action='ListofFeedbackForManager')
		begin	
			Select Garbage_TblFeedback.*,Garbage_TblUser.Name from Garbage_TblFeedback 
			inner join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblFeedback.UserId 
			ORDER by FeedbackId DESC
		end
if(@Action='ListofUserMessageForManager')
		begin	
			Select Garbage_TblUserMessage.* from Garbage_TblUserMessage ORDER by MessageId DESC
		end
if(@Action='InsertMessageUser')
		begin	
			Insert Into Garbage_TblUserMessage(ContactNo,CreatedDate,Email,[Message],Name)values(@ContactNo,GETDATE(),@Email,@Message,@Name)
		end
END

--Client
BEGIN
if(@Action='ListOfRequestForClient')
		begin	
			Select Garbage_TblRequest.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo
			from Garbage_TblRequest
			inner join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblRequest.UserId
			where Garbage_TblRequest.UserId=@UserId

		end
if(@Action='ListOfRequestDetailForClient')
		begin	
			Select Garbage_TblRequestDetail.[Date],Garbage_TblRequestDetail.RequestDetailId,Garbage_TblRequestDetail.RequestDetailStatus,Garbage_TblRequestDetail.[Time],
			 Garbage_TblRequest.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo
			from Garbage_TblRequestDetail
			inner join Garbage_TblRequest on Garbage_TblRequestDetail.RequestId=Garbage_TblRequest.RequestId
			inner join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblRequest.UserId
			where Garbage_TblRequestDetail.RequestId=@RequestId

		end
if(@Action='InsertRequestByClient')
		begin	
			Insert into Garbage_TblRequest([Address],CreatedDate,GarbageType,Lat,Long,[Status],TotalAmount,UserId)
			values(@Address,Getdate(),@GarbageType,@Lat,@Long,0,@TotalAmount,@UserId)
			Set @RequestId =SCOPE_IDENTITY();
			Insert into Garbage_TblRequestDetail([Date],[Time],RequestDetailStatus,RequestId)
			Select [date],[time],0,@RequestId from @RequestDetailType
		end
if(@Action='ConfirmRequestAfterPayementByClient')
		begin	
			Declare @EmployeeId bigint=(Select Top(1) UserId from Garbage_TblUser where Available=1  AND RoleId=2)
			Update Garbage_TblRequest set [Status]=1,EmployeeId=@EmployeeId where RequestId=@RequestId
			Update Garbage_TblRequestDetail set RequestDetailStatus=1 where RequestId=@RequestId
			Update Garbage_TblUser set Available=0 where UserId=@EmployeeId


		end
if(@Action='CloseRequestAfterCollectedByClient')
		begin	
			Update Garbage_TblRequest set [Status]=3,EmployeeId=@EmployeeId where RequestId=@RequestId
			Update Garbage_TblRequestDetail set RequestDetailStatus=3 where RequestId=@RequestId
		end
if(@Action='ListofRateForClient')
		begin	
			Select Garbage_TblRates.* from Garbage_TblRates
		end
if(@Action='ListofFeedbackForClient')
		begin	
			Select Garbage_TblFeedback.*,Garbage_TblUser.Name from Garbage_TblFeedback 
			inner join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblFeedback.UserId
			where Garbage_TblFeedback.UserId=@UserId ORDER by FeedbackId DESC
		end
if(@Action='InsertFeedbackByClient')
		begin	
			Insert into Garbage_TblFeedback(CreatedDate,FeedBack,UserId)values(GETDATE(),@FeedBack,@UserId)
		end
END
--Employee
BEGIN
if(@Action='ListOfPendingRequestForEmployee')
		begin	
			Select Garbage_TblRequest.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo
			from Garbage_TblRequest
			inner join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblRequest.UserId
			where Garbage_TblRequest.EmployeeId=@UserId AND Garbage_TblRequest.[Status]=1

		end
if(@Action='ListOfAllRequestForEmployee')
		begin	
			Select Garbage_TblRequest.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo
			from Garbage_TblRequest
			inner join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblRequest.UserId
			where Garbage_TblRequest.EmployeeId=@UserId

		end
if(@Action='ListOfRequestDetailForEmployee')
		begin	
			Select Garbage_TblRequestDetail.[Date],Garbage_TblRequestDetail.RequestDetailId,Garbage_TblRequestDetail.RequestDetailStatus,Garbage_TblRequestDetail.[Time],
			 Garbage_TblRequest.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo
			from Garbage_TblRequestDetail
			inner join Garbage_TblRequest on Garbage_TblRequestDetail.RequestId=Garbage_TblRequest.RequestId
			inner join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblRequest.UserId
			where Garbage_TblRequestDetail.RequestId=@RequestId

		end
if(@Action='ChangeSubRequestStatusAfterCollectedByEmployee')
		begin	
			Update Garbage_TblRequestDetail set RequestDetailStatus=2 where Garbage_TblRequestDetail.RequestDetailId=@RequestId
			Set @RequestId =(Select Top(1) RequestId from Garbage_TblRequestDetail where RequestDetailId=@RequestId)
			Declare @CheckSubRequest bigint=(Select Top(1) RequestId from Garbage_TblRequestDetail where RequestId=@RequestId AND RequestDetailStatus=1)
			if(@CheckSubRequest is null)
			begin
			Update Garbage_TblRequest set [Status]=2 where RequestId=@RequestId
			Update Garbage_TblUser set Available=1 where UserId=(Select Top(1) EmployeeId from Garbage_TblRequest where RequestId=@RequestId) 
			end
		end
if(@Action='DateWiseReportofRequestForEmployee')
		begin	
			Select Garbage_TblRequest.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo
			from Garbage_TblRequest
			inner join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblRequest.UserId
			where Garbage_TblRequest.EmployeeId=@UserId AND CONVERT(date,CreatedDate)=CONVERT(date,@CreatedDate)

		end
if(@Action='FromToReportofRequestForEmployee')
		begin	
			Select Garbage_TblRequest.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo
			from Garbage_TblRequest
			inner join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblRequest.UserId
			where Garbage_TblRequest.EmployeeId=@UserId AND CONVERT(date,CreatedDate) between CONVERT(date,@DateFrom) AND CONVERT(date,@DateTo)

		end
END


---Company
BEGIN
if(@Action='ListofActiveAuctionForCompany')
		begin	
			Select Garbage_TblAuction.* from Garbage_TblAuction where [Status]=1 
		end
if(@Action='ListofAuctionForOwnByCompany')
		begin	
			Select Garbage_TblAuction.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo from Garbage_TblAuction 
			left join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblAuction.AssignTo  
			where Garbage_TblAuction.AssignTo=@UserId
		end
if(@Action='ListofBiddingOnAuctionForCompany')
		begin	
			Select Garbage_TblCompanyBiding.BidingAmount,Garbage_TblCompanyBiding.BidingId,Garbage_TblCompanyBiding.CompanyId,
			Garbage_TblCompanyBiding.CreatedDate as BiddingCreatedDate, C.Name as CompanyName,C.ContactNo as BiddingContactNo,
			Garbage_TblAuction.*,Garbage_TblUser.Name,Garbage_TblUser.ContactNo from Garbage_TblCompanyBiding
			left join  Garbage_TblAuction on Garbage_TblAuction.AuctionId=Garbage_TblCompanyBiding.AuctionId
			left join Garbage_TblUser on Garbage_TblUser.UserId=Garbage_TblAuction.AssignTo 
			left join Garbage_TblUser C on C.UserId=Garbage_TblCompanyBiding.CompanyId 
			where Garbage_TblCompanyBiding.CompanyId=@UserId

		end
if(@Action='InsertBiddingOnAuctionByCompany')
		begin
			Declare @CheckAuctionDateTime bigint=(Select Top(1) AuctionId from Garbage_TblAuction where AuctionId=@AuctionId AND [Status]=1 AND  Convert(date,DateTo)<Convert(date,GETDATE()))
			if(@CheckAuctionDateTime is not null AND Not @CheckAuctionDateTime='')
			Begin
				Declare @GetCompanyId bigint=(Select Top(1) CompanyId from Garbage_TblCompanyBiding where AuctionId=@CheckAuctionDateTime Order By BidingAmount DESC)
				Update Garbage_TblAuction set [Status]=0,AssignTo=@GetCompanyId where AuctionId=@CheckAuctionDateTime
			END
			Else
			Begin
				Set @CheckAuctionDateTime =(Select Top(1) AuctionId from Garbage_TblAuction where AuctionId=@AuctionId AND [Status]=1 
				AND  Convert(date,DateTo)<=Convert(date,GETDATE()) AND CONVERT(time,TimeTo)<=Convert(time,GETDATE()))
				if(@CheckAuctionDateTime is not null AND Not @CheckAuctionDateTime='')
				begin
					Declare @GetCompanyId1 bigint=(Select Top(1) CompanyId from Garbage_TblCompanyBiding where AuctionId=@CheckAuctionDateTime Order By BidingAmount DESC)
					Update Garbage_TblAuction set [Status]=0,AssignTo=@GetCompanyId1 where AuctionId=@CheckAuctionDateTime
				end
				Else
				Begin
					Declare @CheckAuctionUser bigint=(Select Top(1) CompanyId from Garbage_TblCompanyBiding where AuctionId=@AuctionId AND CompanyId=@UserId)
					if(@CheckAuctionUser is null OR @CheckAuctionUser ='')
					Begin
						Insert Into Garbage_TblCompanyBiding(BidingAmount,CompanyId,CreatedDate,AuctionId)
						values(@BidingAmount,@UserId,GETDATE(),@AuctionId)
					End
				END
			end
		END

END

END

GO
/****** Object:  Table [dbo].[Garbage_TblAuction]    Script Date: 06/30/2020 6:16:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Garbage_TblAuction](
	[AuctionId] [bigint] IDENTITY(1,1) NOT NULL,
	[Detail] [nvarchar](500) NULL,
	[Address] [nvarchar](500) NULL,
	[DateFrom] [date] NULL,
	[DateTo] [date] NULL,
	[TimeFrom] [time](7) NULL,
	[TimeTo] [time](7) NULL,
	[EstimatedRate] [money] NULL,
	[Status] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[AssignTo] [bigint] NULL,
 CONSTRAINT [PK__Garbage___51004A4C22229166] PRIMARY KEY CLUSTERED 
(
	[AuctionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Garbage_TblCompanyBiding]    Script Date: 06/30/2020 6:16:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Garbage_TblCompanyBiding](
	[BidingId] [bigint] IDENTITY(1,1) NOT NULL,
	[AuctionId] [bigint] NULL,
	[CompanyId] [bigint] NULL,
	[BidingAmount] [money] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK__Garbage___CBA5CB7D88916E16] PRIMARY KEY CLUSTERED 
(
	[BidingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Garbage_TblFeedback]    Script Date: 06/30/2020 6:16:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Garbage_TblFeedback](
	[FeedbackId] [bigint] IDENTITY(1,1) NOT NULL,
	[FeedBack] [nvarchar](500) NULL,
	[UserId] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_Garbage_TblFeedback] PRIMARY KEY CLUSTERED 
(
	[FeedbackId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Garbage_TblRates]    Script Date: 06/30/2020 6:16:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Garbage_TblRates](
	[RateId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Rate] [money] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Garbage_TblRequest]    Script Date: 06/30/2020 6:16:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Garbage_TblRequest](
	[RequestId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[Lat] [nvarchar](50) NULL,
	[Long] [nvarchar](50) NULL,
	[Address] [nvarchar](500) NULL,
	[GarbageType] [nvarchar](50) NULL,
	[TotalAmount] [money] NULL,
	[Status] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[EmployeeId] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[RequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Garbage_TblRequestDetail]    Script Date: 06/30/2020 6:16:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Garbage_TblRequestDetail](
	[RequestDetailId] [bigint] IDENTITY(1,1) NOT NULL,
	[RequestId] [bigint] NULL,
	[RequestDetailStatus] [bigint] NULL,
	[Date] [date] NULL,
	[Time] [time](7) NULL,
 CONSTRAINT [PK__Garbage___DC528B90960B1A73] PRIMARY KEY CLUSTERED 
(
	[RequestDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Garbage_TblRoles]    Script Date: 06/30/2020 6:16:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Garbage_TblRoles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Garbage_TblUser]    Script Date: 06/30/2020 6:16:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Garbage_TblUser](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Email] [nvarchar](50) NULL,
	[Password] [nvarchar](500) NULL,
	[Status] [int] NULL,
	[RoleId] [int] NULL,
	[ContactNo] [nvarchar](50) NULL,
	[Available] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Garbage_TblUserMessage]    Script Date: 06/30/2020 6:16:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Garbage_TblUserMessage](
	[MessageId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Email] [nvarchar](50) NULL,
	[Message] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NULL,
	[ContactNo] [nvarchar](20) NULL,
 CONSTRAINT [PK__Garbage___C87C0C9CEA7734DB] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Garbage_TblAuction] ON 

INSERT [dbo].[Garbage_TblAuction] ([AuctionId], [Detail], [Address], [DateFrom], [DateTo], [TimeFrom], [TimeTo], [EstimatedRate], [Status], [CreatedDate], [AssignTo]) VALUES (1, N'Bannu Deri', N'Bannu', CAST(0x3F410B00 AS Date), CAST(0x43410B00 AS Date), CAST(0x070046C323000000 AS Time), CAST(0x07007AA606C90000 AS Time), 50000.0000, 1, CAST(0x0000ABE400ED130C AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Garbage_TblAuction] OFF
SET IDENTITY_INSERT [dbo].[Garbage_TblCompanyBiding] ON 

INSERT [dbo].[Garbage_TblCompanyBiding] ([BidingId], [AuctionId], [CompanyId], [BidingAmount], [CreatedDate]) VALUES (1, 1, 5, 45000.0000, CAST(0x0000ABE40118B360 AS DateTime))
INSERT [dbo].[Garbage_TblCompanyBiding] ([BidingId], [AuctionId], [CompanyId], [BidingAmount], [CreatedDate]) VALUES (2, 1, 5, 46000.0000, CAST(0x0000ABE40118B360 AS DateTime))
INSERT [dbo].[Garbage_TblCompanyBiding] ([BidingId], [AuctionId], [CompanyId], [BidingAmount], [CreatedDate]) VALUES (3, 1, 4, 42000.0000, CAST(0x0000ABE4013ACE0C AS DateTime))
SET IDENTITY_INSERT [dbo].[Garbage_TblCompanyBiding] OFF
SET IDENTITY_INSERT [dbo].[Garbage_TblFeedback] ON 

INSERT [dbo].[Garbage_TblFeedback] ([FeedbackId], [FeedBack], [UserId], [CreatedDate]) VALUES (1, N'Very', 2, CAST(0x0000ABE50115272C AS DateTime))
SET IDENTITY_INSERT [dbo].[Garbage_TblFeedback] OFF
SET IDENTITY_INSERT [dbo].[Garbage_TblRates] ON 

INSERT [dbo].[Garbage_TblRates] ([RateId], [Title], [Rate], [CreatedOn]) VALUES (1, N'Residential', 1800.0000, CAST(0x0000ABE300D2135A AS DateTime))
INSERT [dbo].[Garbage_TblRates] ([RateId], [Title], [Rate], [CreatedOn]) VALUES (2, N'Commercial', 2500.0000, CAST(0x0000ABE300D2962E AS DateTime))
SET IDENTITY_INSERT [dbo].[Garbage_TblRates] OFF
SET IDENTITY_INSERT [dbo].[Garbage_TblRequest] ON 

INSERT [dbo].[Garbage_TblRequest] ([RequestId], [UserId], [Lat], [Long], [Address], [GarbageType], [TotalAmount], [Status], [CreatedDate], [EmployeeId]) VALUES (1, 2, N'78.78', N'56.89', N'abc', N'Residential', 3600.0000, 1, CAST(0x0000ABE400A66B84 AS DateTime), 3)
SET IDENTITY_INSERT [dbo].[Garbage_TblRequest] OFF
SET IDENTITY_INSERT [dbo].[Garbage_TblRequestDetail] ON 

INSERT [dbo].[Garbage_TblRequestDetail] ([RequestDetailId], [RequestId], [RequestDetailStatus], [Date], [Time]) VALUES (1, 1, 1, CAST(0x3F410B00 AS Date), CAST(0x07007870335C0000 AS Time))
INSERT [dbo].[Garbage_TblRequestDetail] ([RequestDetailId], [RequestId], [RequestDetailStatus], [Date], [Time]) VALUES (2, 1, 1, CAST(0x40410B00 AS Date), CAST(0x07007870335C0000 AS Time))
SET IDENTITY_INSERT [dbo].[Garbage_TblRequestDetail] OFF
SET IDENTITY_INSERT [dbo].[Garbage_TblRoles] ON 

INSERT [dbo].[Garbage_TblRoles] ([RoleId], [RoleName]) VALUES (1, N'Manager')
INSERT [dbo].[Garbage_TblRoles] ([RoleId], [RoleName]) VALUES (2, N'Employee')
INSERT [dbo].[Garbage_TblRoles] ([RoleId], [RoleName]) VALUES (3, N'Client')
INSERT [dbo].[Garbage_TblRoles] ([RoleId], [RoleName]) VALUES (4, N'Company')
SET IDENTITY_INSERT [dbo].[Garbage_TblRoles] OFF
SET IDENTITY_INSERT [dbo].[Garbage_TblUser] ON 

INSERT [dbo].[Garbage_TblUser] ([UserId], [Name], [Email], [Password], [Status], [RoleId], [ContactNo], [Available]) VALUES (1, N'Manager', N'manager@gmail.com', N'12345678', 1, 1, N'03319177870', NULL)
INSERT [dbo].[Garbage_TblUser] ([UserId], [Name], [Email], [Password], [Status], [RoleId], [ContactNo], [Available]) VALUES (2, N'Hanif', N'hanif@gmail.com', N'12345678', 1, 3, N'03319177870', 1)
INSERT [dbo].[Garbage_TblUser] ([UserId], [Name], [Email], [Password], [Status], [RoleId], [ContactNo], [Available]) VALUES (3, N'Arshad Mughal', N'11101-6788976-9', N'12345678', 1, 2, N'03319177871', 1)
INSERT [dbo].[Garbage_TblUser] ([UserId], [Name], [Email], [Password], [Status], [RoleId], [ContactNo], [Available]) VALUES (4, N'Mughal 2419', N'mughal2419@gmail.com', N'12345678', 1, 4, N'03319177870', 1)
SET IDENTITY_INSERT [dbo].[Garbage_TblUser] OFF
SET IDENTITY_INSERT [dbo].[Garbage_TblUserMessage] ON 

INSERT [dbo].[Garbage_TblUserMessage] ([MessageId], [Name], [Email], [Message], [CreatedDate], [ContactNo]) VALUES (1, N'Arshad', N'a.mughal2419@gmail.com', N'helo sir', CAST(0x0000ABE5014F6C57 AS DateTime), N'03319177870')
INSERT [dbo].[Garbage_TblUserMessage] ([MessageId], [Name], [Email], [Message], [CreatedDate], [ContactNo]) VALUES (2, N'fhhf', N'arshad@gmail.com', N'heloo', CAST(0x0000ABE5014FB978 AS DateTime), N'+923319177870')
SET IDENTITY_INSERT [dbo].[Garbage_TblUserMessage] OFF
