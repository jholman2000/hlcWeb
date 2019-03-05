SET IDENTITY_INSERT [hlc_Department] ON;
INSERT INTO [hlc_Department] ([Id], [DepartmentName], [DateEntered], [EnteredBy]) VALUES (1, 'Administration', GetDate(), 'jholman');
GO
INSERT INTO [hlc_Department] ([Id], [DepartmentName], [DateEntered], [EnteredBy]) VALUES (2, 'Anesthesiology', GetDate(), 'jholman');
GO
SET IDENTITY_INSERT [hlc_Department] OFF;
