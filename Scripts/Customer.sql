alter table customers add TakePic bit not null default((0))

alter table Appointments add ShareLocation bit not null default((0))

alter table Appointments add JobCompleted bit not null default((0))

alter table Appointments add Notes nvarchar(max) null

alter table Appointments add jobStartTime datetime null

alter table Appointments add jobEndTime datetime null

alter table Appointments add latitude nvarchar(200) null

alter table Appointments add longitude nvarchar(200) null

alter table Contractors add ContractorPic nvarchar(max) null

alter table Franchise add FranchiseImg nvarchar(max) null




alter table Contractors add ShareLocation bit not null default((0))

alter table Contractors add latitude nvarchar(200) null

alter table Contractors add longitude nvarchar(200) null




