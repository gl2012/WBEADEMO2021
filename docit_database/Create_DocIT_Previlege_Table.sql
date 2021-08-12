create table DocIT.dbo.Previlege(
   Previlege_id int PRIMARY KEY IDENTITY(1,1),
   Item_Name VARCHAR(50) NOT NULL,
   user_id int not null,
   Previlege_Edit char(1) ,
   Previlege_View char(1),
   Previlege_Create char(1),
   Createtor int not null,
   DelegateTo int not null,
   Create_Date DATETIME ,
   Edit_Date  Datetime,
   foreign key (user_id) references DocIT.dbo.users
);