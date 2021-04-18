use ClothesDB;
go

merge dbo.Categories as src
using 
(
  select [Name]
       , [Image]
  from (
          values (N'Trousers', 't7.jpg')
               , (N'Pants', 't8.jpg')
               , (N'Jeans', 't3.jpg')
               , (N'Women''s pants and skirts', 't1.jpg')
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
          values (N'Skirt red',199.99,'pi3.png',4)
               , (N'Dark jeans',299.99,'pi10.png',3)
               , (N'Gray pants',349.99,'pi12.png',1)
               , (N'Jeans black',600,'pi9.png',3)
               , (N'Pants black',600,'pi11.png',2)
               , (N'Skirt with polka dots',600,'pi5.png',4)
               , (N'Pants in ornament',449.99,'pi6.png',1)
               , (N'Turn-up jeans',449.99,'pi7.png',3)
               , (N'Black culottes',699.99,'pi8.png',4)
               , (N'Purple pants',399.99,'pi4.png',4)
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
          values (N'Top', '6.jpg')
               , (N'Modern', '12.jpg')
               , (N'Stylish', '13.jpg')
               , (N'Fashionable', '14.jpg')
               , (N'New', '9.jpg')
               , (N'Bright', '10.jpg')
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
          values (N'Andrew', 'andrew@gmail.com', @hash)
               , (N'Kayli', 'kayli@gmail.com', @hash)
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