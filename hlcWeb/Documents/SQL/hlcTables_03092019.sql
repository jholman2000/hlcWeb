/*
Navicat SQL Server Data Transfer

Source Server         : Azure
Source Server Version : 120000
Source Host           : tcp:quagv1i08c.database.windows.net,1433:1433
Source Database       : HLComm
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 120000
File Encoding         : 65001

Date: 2019-03-09 13:06:17
*/


-- ----------------------------
-- Table structure for hlc_Attitude
-- ----------------------------
DROP TABLE [hlc_Attitude]
GO
CREATE TABLE [hlc_Attitude] (
[ID] int NOT NULL ,
[Description] varchar(50) NULL ,
PRIMARY KEY ([ID])
)


GO

-- ----------------------------
-- Indexes structure for table hlc_Attitude
-- ----------------------------

-- ----------------------------
-- Table structure for hlc_CaseFile
-- ----------------------------
DROP TABLE [hlc_CaseFile]
GO
CREATE TABLE [hlc_CaseFile] (
[Id] int NOT NULL ,
[CommitteeId] int NULL DEFAULT ((1)) ,
[CaseDate] smalldatetime NULL ,
[DateEntered] smalldatetime NULL ,
[EnteredBy] varchar(12) NULL ,
[DateLastUpdated] smalldatetime NULL ,
[LastUpdatedBy] varchar(12) NULL ,
[FirstName] varchar(50) NULL ,
[LastName] varchar(50) NULL ,
[Age] int NULL ,
[CongregationName] varchar(80) NULL ,
[DoctorId] int NULL ,
[HospitalId] int NULL ,
[DiagnosisId] int NULL ,
[IsPediatricCase] bit NULL DEFAULT ((0)) ,
[CourtOrderSought] bit NULL DEFAULT ((0)) ,
[CourtOrderSoughtBy] int NULL DEFAULT ((0)) ,
[CourtOrderGranted] bit NULL DEFAULT ((0)) ,
[CourtOrderNotGrantedReason] int NULL DEFAULT ((0)) ,
[CourtOrderComments] varchar(500) NULL ,
[TransfusionGiven] bit NULL DEFAULT ((0)) ,
[ParentsNotified] bit NULL DEFAULT ((0)) ,
[ParentsChargedNeglect] bit NULL DEFAULT ((0)) ,
[ParentsRightsRemoved] bit NULL DEFAULT ((0)) ,
[PatientTransferred] bit NULL DEFAULT ((0)) ,
[TransferDetails] varchar(500) NULL ,
[MedicalHistory] varchar(5000) NULL ,
[TreatmentPlan] varchar(5000) NULL ,
[PossibleStrategies] varchar(5000) NULL ,
[ArticlesShared] varchar(5000) NULL ,
[ConsultingDoctor] varchar(5000) NULL ,
[Outcome] varchar(5000) NULL ,
[AssistingID] int NULL ,
[AnesthID] int NULL 
)


GO

-- ----------------------------
-- Table structure for hlc_Committee
-- ----------------------------
DROP TABLE [hlc_Committee]
GO
CREATE TABLE [hlc_Committee] (
[ID] int NOT NULL ,
[CommitteeName] varchar(80) NULL ,
[City] varchar(50) NULL ,
[State] char(2) NULL ,
[MainContactID] varchar(12) NULL ,
[Notes] varchar(512) NULL ,
PRIMARY KEY ([ID])
)


GO

-- ----------------------------
-- Indexes structure for table hlc_Committee
-- ----------------------------

-- ----------------------------
-- Table structure for hlc_DayOfWeek
-- ----------------------------
DROP TABLE [hlc_DayOfWeek]
GO
CREATE TABLE [hlc_DayOfWeek] (
[Id] int NULL ,
[Description] varchar(50) NULL 
)


GO

-- ----------------------------
-- Table structure for hlc_Department
-- ----------------------------
DROP TABLE [hlc_Department]
GO
CREATE TABLE [hlc_Department] (
[Id] int NOT NULL ,
[DepartmentName] varchar(80) NULL ,
[DateEntered] smalldatetime NULL ,
[EnteredBy] varchar(12) NULL 
)


GO

-- ----------------------------
-- Table structure for hlc_Diagnosis
-- ----------------------------
DROP TABLE [hlc_Diagnosis]
GO
CREATE TABLE [hlc_Diagnosis] (
[Id] int NOT NULL ,
[DiagnosisName] varchar(80) NULL ,
[DateEntered] smalldatetime NULL ,
[EnteredBy] varchar(12) NULL 
)


GO

-- ----------------------------
-- Table structure for hlc_Doctor
-- ----------------------------
DROP TABLE [hlc_Doctor]
GO
CREATE TABLE [hlc_Doctor] (
[ID] int NOT NULL ,
[FirstName] varchar(50) NULL ,
[LastName] varchar(50) NULL ,
[PracticeID] int NULL ,
[MobilePhone] varchar(12) NULL ,
[HomePhone] varchar(12) NULL ,
[Pager] varchar(12) NULL ,
[EmailAddress] varchar(80) NULL ,
[AttitudeCode] char(1) NULL ,
[Attitude] smallint NULL DEFAULT ((9)) ,
[FavAdultEmergency] bit NULL DEFAULT ((0)) ,
[FavAdultNonEmergency] bit NULL DEFAULT ((0)) ,
[NotFavAdult] bit NULL DEFAULT ((0)) ,
[FavChildEmergency] bit NULL DEFAULT ((0)) ,
[FavChildNonEmergency] bit NULL DEFAULT ((0)) ,
[NotFavChild] bit NULL DEFAULT ((0)) ,
[AcceptsMedicaid] bit NULL DEFAULT ((0)) ,
[ConsultAdultEmergency] bit NULL DEFAULT ((0)) ,
[ConsultChildEmergency] bit NULL DEFAULT ((0)) ,
[NOTES] varchar(250) NULL ,
[DOCNOTES] varchar(500) NULL ,
[DateLastUpdated] smalldatetime NULL ,
[LastUpdatedBy] varchar(12) NULL ,
[Status] smallint NULL DEFAULT ((0)) ,
[StatusDate] smalldatetime NULL ,
[IsBSMP] bit NULL DEFAULT ((0)) ,
[RegContacted] smallint NULL DEFAULT ((0)) ,
[SpecificallyKnown] smallint NULL DEFAULT ((0)) ,
[Helpful] smallint NULL DEFAULT ((0)) ,
[FrequentlyTreat] smallint NULL DEFAULT ((0)) ,
[TreatYears] smallint NULL DEFAULT ((0)) ,
[IsHRP] bit NULL DEFAULT ((0)) ,
[PeerReview] varchar(120) NULL ,
[DateEntered] datetime NULL DEFAULT (getdate()) ,
[EnteredBy] varchar(12) NULL ,
PRIMARY KEY ([ID])
)


GO

-- ----------------------------
-- Indexes structure for table hlc_Doctor
-- ----------------------------
CREATE INDEX [IDX_hlc_Doctor_LastName] ON [hlc_Doctor]
([LastName] ASC) 
GO
CREATE INDEX [IDX_hlc_Doctor_PracticeID] ON [hlc_Doctor]
([PracticeID] ASC) 
GO
CREATE INDEX [IDX_hlc_Doctor_Attitude_LastName] ON [hlc_Doctor]
([AttitudeCode] ASC, [LastName] ASC) 
GO
CREATE INDEX [_WA_Sys_Pager_4D4A6ED8] ON [hlc_Doctor]
([Pager] ASC) 
GO
CREATE INDEX [_WA_Sys_MobilePhone_4D4A6ED8] ON [hlc_Doctor]
([MobilePhone] ASC) 
GO
CREATE INDEX [_WA_Sys_HomePhone_4D4A6ED8] ON [hlc_Doctor]
([HomePhone] ASC) 
GO
CREATE INDEX [_WA_Sys_Attitude_4D4A6ED8] ON [hlc_Doctor]
([Attitude] ASC) 
GO
CREATE INDEX [_WA_Sys_DateLastUpdated_4D4A6ED8] ON [hlc_Doctor]
([DateLastUpdated] ASC) 
GO
CREATE INDEX [_WA_Sys_Status_4D4A6ED8] ON [hlc_Doctor]
([Status] ASC) 
GO
CREATE INDEX [_WA_Sys_StatusDate_4D4A6ED8] ON [hlc_Doctor]
([StatusDate] ASC) 
GO
CREATE INDEX [_WA_Sys_PeerReview_4D4A6ED8] ON [hlc_Doctor]
([PeerReview] ASC) 
GO

-- ----------------------------
-- Table structure for hlc_DoctorHospital
-- ----------------------------
DROP TABLE [hlc_DoctorHospital]
GO
CREATE TABLE [hlc_DoctorHospital] (
[ID] int NOT NULL ,
[DoctorID] int NOT NULL ,
[HospitalID] int NOT NULL ,
[Notes] varchar(512) NULL ,
PRIMARY KEY ([ID])
)


GO

-- ----------------------------
-- Indexes structure for table hlc_DoctorHospital
-- ----------------------------
CREATE INDEX [_WA_Sys_DoctorID_44B528D7] ON [hlc_DoctorHospital]
([DoctorID] ASC) 
GO
CREATE INDEX [_WA_Sys_HospitalID_44B528D7] ON [hlc_DoctorHospital]
([HospitalID] ASC) 
GO
CREATE INDEX [IDX_hlc_DoctorHospital_DoctorID] ON [hlc_DoctorHospital]
([DoctorID] ASC) 
GO
CREATE INDEX [IDX_hlc_DoctorHospital_HospitalID] ON [hlc_DoctorHospital]
([HospitalID] ASC) 
GO
CREATE INDEX [_WA_Sys_Notes_44B528D7] ON [hlc_DoctorHospital]
([Notes] ASC) 
GO

-- ----------------------------
-- Table structure for hlc_DoctorNote
-- ----------------------------
DROP TABLE [hlc_DoctorNote]
GO
CREATE TABLE [hlc_DoctorNote] (
[ID] int NOT NULL ,
[DoctorID] int NULL ,
[UserID] varchar(12) NULL ,
[DateEntered] smalldatetime NULL ,
[Notes] varchar(5000) NULL ,
[FileName] varchar(80) NULL ,
PRIMARY KEY ([ID])
)


GO

-- ----------------------------
-- Indexes structure for table hlc_DoctorNote
-- ----------------------------
CREATE INDEX [_WA_Sys_DoctorID_2F8501C7] ON [hlc_DoctorNote]
([DoctorID] ASC) 
GO
CREATE INDEX [_WA_Sys_UserID_2F8501C7] ON [hlc_DoctorNote]
([UserID] ASC) 
GO
CREATE INDEX [_WA_Sys_DateEntered_2F8501C7] ON [hlc_DoctorNote]
([DateEntered] ASC) 
GO
CREATE INDEX [_WA_Sys_FileName_2F8501C7] ON [hlc_DoctorNote]
([FileName] ASC) 
GO

-- ----------------------------
-- Table structure for hlc_DoctorSpecialty
-- ----------------------------
DROP TABLE [hlc_DoctorSpecialty]
GO
CREATE TABLE [hlc_DoctorSpecialty] (
[ID] int NOT NULL ,
[DoctorID] int NULL ,
[SpecialtyID] int NULL ,
[AreaOfExpertise] varchar(250) NULL ,
PRIMARY KEY ([ID])
)


GO

-- ----------------------------
-- Indexes structure for table hlc_DoctorSpecialty
-- ----------------------------
CREATE INDEX [_WA_Sys_DoctorID_3FF073BA] ON [hlc_DoctorSpecialty]
([DoctorID] ASC) 
GO
CREATE INDEX [_WA_Sys_SpecialtyID_3FF073BA] ON [hlc_DoctorSpecialty]
([SpecialtyID] ASC) 
GO
CREATE INDEX [IDX_hlc_DoctorSpecialty_DoctorID] ON [hlc_DoctorSpecialty]
([DoctorID] ASC) 
GO
CREATE INDEX [IDX_hlc_DoctorSpecialty_SpecialtyID] ON [hlc_DoctorSpecialty]
([SpecialtyID] ASC) 
GO
CREATE INDEX [_WA_Sys_AreaOfExpertise_3FF073BA] ON [hlc_DoctorSpecialty]
([AreaOfExpertise] ASC) 
GO

-- ----------------------------
-- Table structure for hlc_ErrorLog
-- ----------------------------
DROP TABLE [hlc_ErrorLog]
GO
CREATE TABLE [hlc_ErrorLog] (
[DateError] smalldatetime NULL ,
[Message] varchar(5000) NULL ,
[Data] varchar(5000) NULL ,
[StackTrace] varchar(5000) NULL 
)


GO

-- ----------------------------
-- Table structure for hlc_FacilityType
-- ----------------------------
DROP TABLE [hlc_FacilityType]
GO
CREATE TABLE [hlc_FacilityType] (
[Id] int NULL ,
[Description] varchar(50) NULL 
)


GO

-- ----------------------------
-- Table structure for hlc_Hospital
-- ----------------------------
DROP TABLE [hlc_Hospital]
GO
CREATE TABLE [hlc_Hospital] (
[ID] int NOT NULL ,
[HospitalName] varchar(80) NULL ,
[City] varchar(50) NULL ,
[State] char(2) NULL ,
[CommitteeID] int NULL ,
[Address1] varchar(80) NULL ,
[Address2] varchar(80) NULL ,
[Zip] varchar(10) NULL ,
[OfficePhone1] varchar(14) NULL ,
[Fax] varchar(14) NULL ,
[Notes] varchar(2000) NULL ,
[HasBSMP] bit NULL DEFAULT ((0)) ,
[BSMPFirstName] varchar(50) NULL ,
[BSMPLastName] varchar(50) NULL ,
[BSMPAddress] varchar(80) NULL ,
[BSMPCity] varchar(50) NULL ,
[BSMPState] varchar(2) NULL ,
[BSMPZip] varchar(10) NULL ,
[BSMPEmailAddress] varchar(80) NULL ,
[BSMPHomePhone] varchar(14) NULL ,
[BSMPMobilePhone] varchar(14) NULL ,
[BSMPNotes] varchar(2000) NULL ,
[HospitalType] int NULL ,
[HasPediatrics] bit NULL DEFAULT ((0)) ,
[BMSPCoordName] varchar(80) NULL ,
[BMSPCoordPhone] varchar(12) NULL ,
[BMSPCoordIsWitness] bit NULL DEFAULT ((0)) ,
[BMSPCommitment] varchar(5000) NULL ,
[BMSPSpecialties] varchar(5000) NULL ,
[BMSPPhone] varchar(12) NULL ,
[BMSPNumberOfDoctors] varchar(50) NULL ,
[DateEntered] datetime NULL DEFAULT (getdate()) ,
[EnteredBy] varchar(12) NULL ,
[DateLastUpdated] datetime NULL DEFAULT (getdate()) ,
[LastUpdatedBy] varchar(12) NULL ,
[OftenReceivesWitnesses] bit NULL DEFAULT ((0)) ,
PRIMARY KEY ([ID])
)


GO

-- ----------------------------
-- Indexes structure for table hlc_Hospital
-- ----------------------------
CREATE INDEX [_WA_Sys_HospitalName_2FBA0BF1] ON [hlc_Hospital]
([HospitalName] ASC) 
GO
CREATE INDEX [_WA_Sys_City_2FBA0BF1] ON [hlc_Hospital]
([City] ASC) 
GO
CREATE INDEX [_WA_Sys_State_2FBA0BF1] ON [hlc_Hospital]
([State] ASC) 
GO
CREATE INDEX [_WA_Sys_ComitteeID_2FBA0BF1] ON [hlc_Hospital]
([CommitteeID] ASC) 
GO
CREATE INDEX [IDX_hlc_Hospital_HospitalName] ON [hlc_Hospital]
([HospitalName] ASC) 
GO

-- ----------------------------
-- Table structure for hlc_Hospitality
-- ----------------------------
DROP TABLE [hlc_Hospitality]
GO
CREATE TABLE [hlc_Hospitality] (
[ID] int NOT NULL ,
[FirstName] varchar(50) NULL ,
[Lastname] varchar(50) NULL ,
[Address1] varchar(50) NULL ,
[City] varchar(50) NULL ,
[State] char(2) NULL ,
[Zip] varchar(10) NULL ,
[HospitalID] int NULL ,
[Hospital] varchar(50) NULL ,
[MILES2HOSP] int NULL ,
[Phone] varchar(12) NULL ,
[AltPhone] varchar(12) NULL ,
[AltPhone2] varchar(12) NULL ,
[EmailAddress] varchar(80) NULL ,
[SpeaksEnglish] bit NULL DEFAULT ((1)) ,
[SpeaksSpanish] bit NULL DEFAULT ((0)) ,
[OtherLanguage] varchar(15) NULL ,
[CongregationName] varchar(50) NULL ,
[IsMarried] bit NULL DEFAULT ((0)) ,
[IsSingleSister] bit NULL DEFAULT ((0)) ,
[IsElder] bit NULL DEFAULT ((0)) ,
[IsServant] bit NULL DEFAULT ((0)) ,
[IsPioneer] bit NULL DEFAULT ((0)) ,
[HasCats] bit NULL DEFAULT ((0)) ,
[HasDogs] bit NULL DEFAULT ((0)) ,
[HasOtherPets] bit NULL DEFAULT ((0)) ,
[HasPrivateEntrance] bit NULL DEFAULT ((0)) ,
[HasPrivateRoom] bit NULL DEFAULT ((0)) ,
[HasPrivateBath] bit NULL DEFAULT ((0)) ,
[HasPrivateKey] bit NULL DEFAULT ((0)) ,
[Notes] varchar(512) NULL ,
[DateLastUpdated] smalldatetime NULL ,
[LastUpdatedBy] varchar(12) NULL ,
PRIMARY KEY ([ID])
)


GO

-- ----------------------------
-- Indexes structure for table hlc_Hospitality
-- ----------------------------
CREATE INDEX [_WA_Sys_FirstName_67FE6514] ON [hlc_Hospitality]
([FirstName] ASC) 
GO
CREATE INDEX [_WA_Sys_Lastname_67FE6514] ON [hlc_Hospitality]
([Lastname] ASC) 
GO
CREATE INDEX [_WA_Sys_Address1_67FE6514] ON [hlc_Hospitality]
([Address1] ASC) 
GO
CREATE INDEX [_WA_Sys_City_67FE6514] ON [hlc_Hospitality]
([City] ASC) 
GO
CREATE INDEX [_WA_Sys_State_67FE6514] ON [hlc_Hospitality]
([State] ASC) 
GO
CREATE INDEX [_WA_Sys_Zip_67FE6514] ON [hlc_Hospitality]
([Zip] ASC) 
GO
CREATE INDEX [_WA_Sys_HospitalID_67FE6514] ON [hlc_Hospitality]
([HospitalID] ASC) 
GO
CREATE INDEX [_WA_Sys_Hospital_67FE6514] ON [hlc_Hospitality]
([Hospital] ASC) 
GO
CREATE INDEX [_WA_Sys_MILES2HOSP_67FE6514] ON [hlc_Hospitality]
([MILES2HOSP] ASC) 
GO
CREATE INDEX [_WA_Sys_Phone_67FE6514] ON [hlc_Hospitality]
([Phone] ASC) 
GO
CREATE INDEX [_WA_Sys_AltPhone_67FE6514] ON [hlc_Hospitality]
([AltPhone] ASC) 
GO
CREATE INDEX [_WA_Sys_AltPhone2_67FE6514] ON [hlc_Hospitality]
([AltPhone2] ASC) 
GO
CREATE INDEX [_WA_Sys_EmailAddress_67FE6514] ON [hlc_Hospitality]
([EmailAddress] ASC) 
GO
CREATE INDEX [_WA_Sys_SpeaksEnglish_67FE6514] ON [hlc_Hospitality]
([SpeaksEnglish] ASC) 
GO
CREATE INDEX [_WA_Sys_SpeaksSpanish_67FE6514] ON [hlc_Hospitality]
([SpeaksSpanish] ASC) 
GO
CREATE INDEX [_WA_Sys_OtherLanguage_67FE6514] ON [hlc_Hospitality]
([OtherLanguage] ASC) 
GO
CREATE INDEX [_WA_Sys_CongregationName_67FE6514] ON [hlc_Hospitality]
([CongregationName] ASC) 
GO
CREATE INDEX [_WA_Sys_IsMarried_67FE6514] ON [hlc_Hospitality]
([IsMarried] ASC) 
GO
CREATE INDEX [_WA_Sys_IsSingleSister_67FE6514] ON [hlc_Hospitality]
([IsSingleSister] ASC) 
GO
CREATE INDEX [_WA_Sys_IsElder_67FE6514] ON [hlc_Hospitality]
([IsElder] ASC) 
GO
CREATE INDEX [_WA_Sys_IsServant_67FE6514] ON [hlc_Hospitality]
([IsServant] ASC) 
GO
CREATE INDEX [_WA_Sys_IsPioneer_67FE6514] ON [hlc_Hospitality]
([IsPioneer] ASC) 
GO
CREATE INDEX [_WA_Sys_HasCats_67FE6514] ON [hlc_Hospitality]
([HasCats] ASC) 
GO
CREATE INDEX [_WA_Sys_HasDogs_67FE6514] ON [hlc_Hospitality]
([HasDogs] ASC) 
GO
CREATE INDEX [_WA_Sys_HasOtherPets_67FE6514] ON [hlc_Hospitality]
([HasOtherPets] ASC) 
GO
CREATE INDEX [_WA_Sys_HasPrivateEntrance_67FE6514] ON [hlc_Hospitality]
([HasPrivateEntrance] ASC) 
GO
CREATE INDEX [_WA_Sys_HasPrivateRoom_67FE6514] ON [hlc_Hospitality]
([HasPrivateRoom] ASC) 
GO
CREATE INDEX [_WA_Sys_HasPrivateBath_67FE6514] ON [hlc_Hospitality]
([HasPrivateBath] ASC) 
GO
CREATE INDEX [_WA_Sys_HasPrivateKey_67FE6514] ON [hlc_Hospitality]
([HasPrivateKey] ASC) 
GO
CREATE INDEX [_WA_Sys_Notes_67FE6514] ON [hlc_Hospitality]
([Notes] ASC) 
GO
CREATE INDEX [_WA_Sys_DateLastUpdated_67FE6514] ON [hlc_Hospitality]
([DateLastUpdated] ASC) 
GO
CREATE INDEX [_WA_Sys_LastUpdatedBy_67FE6514] ON [hlc_Hospitality]
([LastUpdatedBy] ASC) 
GO

-- ----------------------------
-- Table structure for hlc_HospitalType
-- ----------------------------
DROP TABLE [hlc_HospitalType]
GO
CREATE TABLE [hlc_HospitalType] (
[Id] int NULL ,
[Description] varchar(50) NULL 
)


GO

-- ----------------------------
-- Table structure for hlc_Practice
-- ----------------------------
DROP TABLE [hlc_Practice]
GO
CREATE TABLE [hlc_Practice] (
[ID] int NOT NULL ,
[PracticeName] varchar(80) NULL ,
[Address1] varchar(80) NULL ,
[Address2] varchar(80) NULL ,
[Address3] varchar(80) NULL ,
[City] varchar(50) NULL ,
[State] char(2) NULL ,
[Zip] varchar(10) NULL ,
[OfficePhone1] varchar(12) NULL ,
[OfficePhone2] varchar(12) NULL ,
[Fax] varchar(12) NULL ,
[WebsiteURL] varchar(250) NULL ,
[OfficeManager] varchar(50) NULL ,
[FacilityType] int NULL DEFAULT ((0)) ,
[Notes] varchar(5000) NULL ,
PRIMARY KEY ([ID])
)


GO

-- ----------------------------
-- Indexes structure for table hlc_Practice
-- ----------------------------
CREATE INDEX [_WA_Sys_PracticeName_3A379A64] ON [hlc_Practice]
([PracticeName] ASC) 
GO
CREATE INDEX [_WA_Sys_Address1_3A379A64] ON [hlc_Practice]
([Address1] ASC) 
GO
CREATE INDEX [_WA_Sys_Address2_3A379A64] ON [hlc_Practice]
([Address2] ASC) 
GO
CREATE INDEX [_WA_Sys_Address3_3A379A64] ON [hlc_Practice]
([Address3] ASC) 
GO
CREATE INDEX [_WA_Sys_City_3A379A64] ON [hlc_Practice]
([City] ASC) 
GO
CREATE INDEX [_WA_Sys_State_3A379A64] ON [hlc_Practice]
([State] ASC) 
GO
CREATE INDEX [_WA_Sys_Zip_3A379A64] ON [hlc_Practice]
([Zip] ASC) 
GO
CREATE INDEX [_WA_Sys_OfficePhone1_3A379A64] ON [hlc_Practice]
([OfficePhone1] ASC) 
GO
CREATE INDEX [_WA_Sys_OfficePhone2_3A379A64] ON [hlc_Practice]
([OfficePhone2] ASC) 
GO
CREATE INDEX [_WA_Sys_Fax_3A379A64] ON [hlc_Practice]
([Fax] ASC) 
GO
CREATE INDEX [_WA_Sys_WebsiteURL_3A379A64] ON [hlc_Practice]
([WebsiteURL] ASC) 
GO
CREATE INDEX [_WA_Sys_OfficeManager_3A379A64] ON [hlc_Practice]
([OfficeManager] ASC) 
GO

-- ----------------------------
-- Table structure for hlc_Presentation
-- ----------------------------
DROP TABLE [hlc_Presentation]
GO
CREATE TABLE [hlc_Presentation] (
[Id] int NOT NULL ,
[Description] varchar(120) NULL ,
[DateEntered] smalldatetime NULL ,
[EnteredBy] varchar(12) NULL ,
[DateLastUpdated] smalldatetime NULL ,
[LastUpdatedBy] varchar(12) NULL ,
[PresentationFacilityType] int NULL DEFAULT ((99)) ,
[FacilityId] int NULL ,
[DepartmentId] int NULL ,
[ContactName] varchar(80) NULL ,
[ContactTitle] varchar(80) NULL ,
[ContactAddress1] varchar(80) NULL ,
[ContactAddress2] varchar(80) NULL ,
[ContactCity] varchar(50) NULL ,
[ContactState] char(2) NULL ,
[ContactZip] varchar(10) NULL ,
[ContactPhone] varchar(12) NULL ,
[ContactEmailAddress] varchar(80) NULL ,
[CoordinatorId] varchar(12) NULL ,
[HLCAssigned] varchar(5000) NULL ,
[Goal] varchar(5000) NULL ,
[Notes] varchar(5000) NULL ,
[ReferredBy] varchar(5000) NULL ,
[IsCVRequired] bit NULL DEFAULT ((0)) ,
[DatePlanned] smalldatetime NULL ,
[DatePreparation] smalldatetime NULL ,
[DatePresented] smalldatetime NULL ,
[Audience] varchar(250) NULL ,
[Address] varchar(80) NULL ,
[PresenterId] varchar(12) NULL ,
[HLCAttended] varchar(5000) NULL ,
[PVGElders] varchar(5000) NULL ,
[PresentationTopic] varchar(5000) NULL ,
[QuestionsAddressed] varchar(5000) NULL ,
[MaterialGiven] varchar(5000) NULL ,
[FollowUpTasks] varchar(5000) NULL 
)


GO

-- ----------------------------
-- Table structure for hlc_PVGMember
-- ----------------------------
DROP TABLE [hlc_PVGMember]
GO
CREATE TABLE [hlc_PVGMember] (
[ID] int NOT NULL ,
[FirstName] varchar(50) NULL ,
[LastName] varchar(50) NULL ,
[Address] varchar(50) NULL ,
[City] varchar(50) NULL ,
[State] char(2) NULL ,
[Zip] varchar(10) NULL ,
[HomePhone] varchar(12) NULL ,
[MobilePhone] varchar(12) NULL ,
[EmailAddress] varchar(80) NULL ,
[Notes] varchar(1500) NULL ,
[Congregation] varchar(50) NULL ,
[DateEntered] datetime NULL DEFAULT (getdate()) ,
[EnteredBy] varchar(12) NULL ,
[DateLastUpdated] datetime NULL ,
[LastUpdatedBy] varchar(12) NULL ,
PRIMARY KEY ([ID])
)


GO

-- ----------------------------
-- Indexes structure for table hlc_PVGMember
-- ----------------------------

-- ----------------------------
-- Table structure for hlc_PVGMemberHospital
-- ----------------------------
DROP TABLE [hlc_PVGMemberHospital]
GO
CREATE TABLE [hlc_PVGMemberHospital] (
[ID] int NOT NULL ,
[PVGMemberID] int NULL ,
[HospitalID] int NULL ,
[DayOfWeek] smallint NULL DEFAULT ((0)) ,
[Notes] varchar(250) NULL ,
PRIMARY KEY ([ID])
)


GO

-- ----------------------------
-- Indexes structure for table hlc_PVGMemberHospital
-- ----------------------------
CREATE INDEX [_WA_Sys_PVGMemberID_65E11278] ON [hlc_PVGMemberHospital]
([PVGMemberID] ASC) 
GO
CREATE INDEX [_WA_Sys_HospitalID_65E11278] ON [hlc_PVGMemberHospital]
([HospitalID] ASC) 
GO

-- ----------------------------
-- Table structure for hlc_Specialty
-- ----------------------------
DROP TABLE [hlc_Specialty]
GO
CREATE TABLE [hlc_Specialty] (
[ID] int NOT NULL ,
[SpecialtyName] varchar(50) NULL ,
[SpecialtyCode] char(3) NULL ,
PRIMARY KEY ([ID])
)


GO

-- ----------------------------
-- Indexes structure for table hlc_Specialty
-- ----------------------------
CREATE INDEX [_WA_Sys_SpecialtyName_31A25463] ON [hlc_Specialty]
([SpecialtyName] ASC) 
GO

-- ----------------------------
-- Table structure for hlc_User
-- ----------------------------
DROP TABLE [hlc_User]
GO
CREATE TABLE [hlc_User] (
[UserID] varchar(12) NOT NULL ,
[CommitteeID] int NULL ,
[FirstName] varchar(50) NULL ,
[LastName] varchar(50) NULL ,
[Password] varchar(50) NULL ,
[UserRole] varchar(12) NULL ,
[EmailAddress] varchar(80) NULL ,
[Address] varchar(80) NULL ,
[City] varchar(50) NULL ,
[State] char(2) NULL ,
[Zip] varchar(10) NULL ,
[HomePhone] varchar(12) NULL ,
[CellPhone] varchar(12) NULL ,
[DateLastOn] smalldatetime NULL ,
[IsActive] bit NULL DEFAULT ((1)) ,
[MustChangePassword] bit NULL DEFAULT ((1)) ,
PRIMARY KEY ([UserID])
)


GO

-- ----------------------------
-- Indexes structure for table hlc_User
-- ----------------------------
CREATE INDEX [_WA_Sys_CommitteeID_4885B9BB] ON [hlc_User]
([CommitteeID] ASC) 
GO
CREATE INDEX [_WA_Sys_FirstName_4885B9BB] ON [hlc_User]
([FirstName] ASC) 
GO
CREATE INDEX [_WA_Sys_LastName_4885B9BB] ON [hlc_User]
([LastName] ASC) 
GO
CREATE INDEX [_WA_Sys_Password_4885B9BB] ON [hlc_User]
([Password] ASC) 
GO
CREATE INDEX [_WA_Sys_UserRole_4885B9BB] ON [hlc_User]
([UserRole] ASC) 
GO
CREATE INDEX [_WA_Sys_EmailAddress_4885B9BB] ON [hlc_User]
([EmailAddress] ASC) 
GO
CREATE INDEX [_WA_Sys_Address_4885B9BB] ON [hlc_User]
([Address] ASC) 
GO
CREATE INDEX [_WA_Sys_City_4885B9BB] ON [hlc_User]
([City] ASC) 
GO
CREATE INDEX [_WA_Sys_State_4885B9BB] ON [hlc_User]
([State] ASC) 
GO
CREATE INDEX [_WA_Sys_Zip_4885B9BB] ON [hlc_User]
([Zip] ASC) 
GO
CREATE INDEX [_WA_Sys_HomePhone_4885B9BB] ON [hlc_User]
([HomePhone] ASC) 
GO
CREATE INDEX [_WA_Sys_CellPhone_4885B9BB] ON [hlc_User]
([CellPhone] ASC) 
GO
CREATE INDEX [_WA_Sys_DateLastOn_4885B9BB] ON [hlc_User]
([DateLastOn] ASC) 
GO
CREATE INDEX [_WA_Sys_IsActive_4885B9BB] ON [hlc_User]
([IsActive] ASC) 
GO
CREATE INDEX [_WA_Sys_MustChangePassword_4885B9BB] ON [hlc_User]
([MustChangePassword] ASC) 
GO

-- ----------------------------
-- Function structure for fn_MonthName
-- ----------------------------
DROP FUNCTION [fn_MonthName]
GO
CREATE FUNCTION [fn_MonthName]
( 
  @MonthNumber smallint,
  @Abbreviation bit = 1
)
RETURNS varchar(10)
AS
BEGIN
declare @Return varchar(10)
select @Return = case @MonthNumber
    when 1  then 'January'
    when 2  then 'February'
    when 3  then 'March'
    when 4  then 'April'
    when 5  then 'May'
    when 6  then 'June'
    when 7  then 'July'
    when 8  then 'August'
    when 9  then 'September'
    when 10 then 'October'
    when 11 then 'November'
    when 12 then 'December'
    else 'Unknown'
    end

if @Abbreviation = 1
    set @Return = Left(@Return, 3)

return @Return
END

GO

-- ----------------------------
-- Function structure for hlc_AttitudeText
-- ----------------------------
DROP FUNCTION [hlc_AttitudeText]
GO
CREATE FUNCTION [hlc_AttitudeText] 
(@Attitude smallint)
RETURNS varchar(20)
AS
BEGIN
declare @Return varchar(20)
select @Return = case @Attitude
    when 0 then 'Unknown'
    when 1 then 'Cooperative'
    when 2 then 'Favorable'
    when 3 then 'Limitations'
    when 4 then 'NOT Favorable'
    when 5 then 'Out of Area Consultant'
    else '???'
    end

return @return
end



GO

-- ----------------------------
-- Function structure for hlc_DayOfWeekText
-- ----------------------------
DROP FUNCTION [hlc_DayOfWeekText]
GO
CREATE FUNCTION [hlc_DayOfWeekText]  
(@DayOfWeek smallint)
RETURNS varchar(10)
AS
BEGIN
declare @Return varchar(10)
select @Return = case @DayOfWeek
    when 1 then 'Sunday'
    when 2 then 'Monday'
    when 3 then 'Tuesday'
    when 4 then 'Wednesday'
    when 5 then 'Thursday'
    when 6 then 'Friday'
    when 7 then 'Saturday'
    when 8 then 'As Needed'
    when 9 then 'Alternate'
    else 'Unknown'
    end

return @return
end


GO

-- ----------------------------
-- Function structure for hlc_GetHospitals
-- ----------------------------
DROP FUNCTION [hlc_GetHospitals]
GO
CREATE FUNCTION [hlc_GetHospitals] (@DoctorID int)  
RETURNS varchar(2000) AS  

BEGIN 
  DECLARE @Output VARCHAR(2000)
  SET @Output = ''

  SELECT @Output = CASE @Output
                   WHEN '' THEN HospitalName
                   ELSE @Output + ', ' + HospitalName
                   END
  FROM hlc_DoctorHospital INNER JOIN
       hlc_Hospital ON hlc_DoctorHospital.HospitalID = hlc_Hospital.ID
  WHERE hlc_DoctorHospital.DoctorID = @DoctorID

RETURN @Output
END


GO

-- ----------------------------
-- Function structure for hlc_GetSpecialties
-- ----------------------------
DROP FUNCTION [hlc_GetSpecialties]
GO
CREATE FUNCTION [hlc_GetSpecialties] (@DoctorID int)  
RETURNS varchar(2000) AS  

BEGIN 
  DECLARE @Output VARCHAR(2000)
  SET @Output = ''

  SELECT @Output = CASE @Output
                   WHEN '' THEN SpecialtyName
                   ELSE @Output + ', ' + SpecialtyName
                   END
  FROM hlc_DoctorSpecialty INNER JOIN
       hlc_Specialty ON hlc_DoctorSpecialty.SpecialtyID = hlc_Specialty.ID
  WHERE hlc_DoctorSpecialty.DoctorID = @DoctorID

RETURN @Output
END

GO

-- ----------------------------
-- Function structure for hlc_StatusText
-- ----------------------------
DROP FUNCTION [hlc_StatusText]
GO
CREATE FUNCTION [hlc_StatusText] 
(@Status smallint)
RETURNS varchar(20)
AS
BEGIN
declare @Return varchar(20)
select @Return = case @Status
    when 0 then 'Unknown'
    when 1 then 'Newly identified'
    when 2 then 'Intro letter sent'
    when 6 then 'Retired'
    when 7 then 'Deceased'
    when 8 then 'Moved out of area'
    when 9 then 'Active'
    when 10 then 'Retired'  -- 12/3/2015 per Mark Jones request
    else '???'
    end

return @return
end


GO
