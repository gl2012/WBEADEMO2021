create table DocIT.dbo.User_SampleTypes(
   id int PRIMARY KEY IDENTITY(1,1), 
   user_id int not null,
   sample_type_id int not null,
   date_modified DATETIME ,
   foreign key (user_id) references DocIT.dbo.users
);