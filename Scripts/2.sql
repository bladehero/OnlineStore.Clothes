use ClothesDB;
go

merge dbo.Categories as src
using 
(
  select [Name]
       , [Image]
  from (
          values (N'Штаны', 't7.jpg')
               , (N'Брюки', 't8.jpg')
               , (N'Джинсы', 't3.jpg')
               , (N'Женские штаны и юбки', 't1.jpg')
       ) as v([Name], [Image])
) as trg
  on src.[Name] = trg.[Name]
when not matched by target then
  insert ([Name], [Image]) 
  values (trg.[Name], trg.[Image]);
go

merge dbo.Products as src
using 
(
  select [Name]
       , Price
       , [Image]
       , CategoryId
  from (
          values (N'Юбка красная"',199.99,'pi3.png',4)
               , (N'Джинсы темные',299.99,'pi10.png',3)
               , (N'Серые штаны',349.99,'pi12.png',1)
               , (N'Джинсы черные',600,'pi9.png',3)
               , (N'Брюки черные',600,'pi11.png',2)
               , (N'Юбка в горошек',600,'pi5.png',4)
               , (N'Штаны в орнамент',449.99,'pi6.png',1)
               , (N'Джинсы с подворотами',449.99,'pi7.png',3)
               , (N'Кюлоты черные',699.99,'pi8.png',4)
               , (N'Фиолетовые штаны',399.99,'pi4.png',4)
       ) as v([Name], Price, [Image], CategoryId)
) as trg
  on src.[Name] = trg.[Name]
when not matched by target then
  insert ([Name], Price, [Image], CategoryId) 
  values (trg.[Name], trg.Price, trg.[Image], trg.CategoryId);
go

merge dbo.Tags as src
using 
(
  select [Name]
       , [Image]
  from (
          values (N'Брендовое', '6.jpg')
               , (N'Современное', '12.jpg')
               , (N'Стильное', '13.jpg')
               , (N'Модное', '14.jpg')
               , (N'Новое', '9.jpg')
               , (N'Яркое', '10.jpg')
       ) as v([Name], [Image])
) as trg
  on src.[Name] = trg.[Name]
when not matched by target then
  insert ([Name], [Image]) 
  values (trg.[Name], trg.[Image]);
go

declare @hash varchar(32) = convert(varchar(32), hashbytes('MD5', 'qwerty'), 2);
merge dbo.Users as src
using 
(
  select FirstName
       , Email
       , [Password]
  from (
          values (N'Андрей', 'andrew@gmail.com', @hash)
               , (N'Василиса', 'vasilisa@gmail.com', @hash)
       ) as v(FirstName, Email, [Password])
) as trg
  on src.Email = trg.Email
when not matched by target then
  insert (FirstName, Email, [Password]) 
  values (trg.FirstName, trg.Email, trg.[Password]);
go

merge dbo.TaggedProducts as src
using 
(
  select ProductId
       , TagId
  from (
          values (1,1)
               , (2,1)
               , (3,1)
               , (4,1)
               , (5,1)
               , (6,1)
               , (7,1)
               , (8,1)
               , (9,1)
               , (10,1)
               , (1,2)
               , (2,3)
               , (3,4)
               , (4,5)
               , (5,6)
               , (6,2)
               , (7,3)
               , (8,4)
               , (9,5)
               , (10,6)
       ) as v(ProductId, TagId)
) as trg
  on src.ProductId = trg.ProductId
 and src.TagId = trg.TagId
when not matched by target then
  insert (ProductId, TagId) 
  values (trg.ProductId, trg.TagId);
go