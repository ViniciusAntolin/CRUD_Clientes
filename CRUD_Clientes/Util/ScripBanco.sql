CREATE TABLE Clientes (
	CodigoCliente BIGINT NOT NULL IDENTITY(1,1),
	Nome VARCHAR(30),
	Sobrenome VARCHAR(30),
	CodigoGenero TINYINT NOT NULL CHECK (CodigoGenero IN('1', '2', '5')),
	DataNascimento Date NOT NULL,
	Endereco VARCHAR(MAX),
	Numero int
)
GO

CREATE TABLE Generos (
	CodigoGenero TINYINT NOT NULL IDENTITY(1,1),
	Descricao VARCHAR(10) NOT NULL CHECK (Descricao IN('Masculino', 'Feminino', 'Outro'))
)
GO

ALTER TABLE CLIENTES 
ADD CONSTRAINT PK_CLIENTE
PRIMARY KEY (CodigoCliente)
GO

ALTER TABLE Generos
ADD CONSTRAINT PK_GENERO
PRIMARY KEY (CodigoGenero)
GO

ALTER TABLE CLIENTES 
ADD CONSTRAINT FK_CLIENTE_GENERO
FOREIGN KEY (CodigoGenero)
REFERENCES Generos(CodigoGenero)
GO


INSERT INTO Generos VALUES ('Masculino')
GO

INSERT INTO Generos VALUES ('Feminino')
GO

INSERT INTO Generos VALUES ('Outro')
GO