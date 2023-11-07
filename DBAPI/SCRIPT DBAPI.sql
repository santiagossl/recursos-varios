create database DBAPI

USE DBAPI

CREATE TABLE CATEGORIA
(
IdCategoria			int primary key identity(1,1) ,
Descripcion			varchar(50)
)


CREATE TABLE PRODUCTO
(
IdProducto int primary key  identity(1,1) ,
CodigoBarra varchar(20),
Descripcion varchar(50),
Marca varchar(50),
IdCategoria int,
Precio decimal (10,2)
constraint FK_IDCATEGORIA FOREIGN KEY (IdCategoria) REFERENCES CATEGORIA (IdCategoria)
)



INSERT INTO CATEGORIA (Descripcion) VALUES 
('Tecnologia'),
('ElectroHogar'),
('Accesorios')


INSERT INTO PRODUCTO (CodigoBarra , Descripcion ,Marca ,IdCategoria ,Precio   ) VALUES 
('5091010', 'Monitor Aoc-curvo','AOC',1,1200),
('5091012', 'Lavadora Lg lg123', 'LG',2,2300)



select * from PRODUCTO

select * from CATEGORIA