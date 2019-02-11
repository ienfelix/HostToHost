
USE HostToHost
go

CREATE TABLE Usuario
(
IdUsuario nchar(6) not null,
ApePaterno nvarchar(50) not null,
ApeMaterno nvarchar(50) not null,
Nombres nvarchar(50) not null,
Correo nvarchar(50) not null,
Celular nvarchar(15) not null,
Estado bit not null,
UsuarioCreacion nvarchar(20) not null,
FechaCreacion datetime not null,
UsuarioEdicion nvarchar(20),
FechaEdicion datetime,
PRIMARY KEY(IdUsuario)
);

INSERT INTO Usuario (IdUsuario, ApePaterno, ApeMaterno, Nombres, Correo, Celular, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('000001', 'Sueldo', 'Villavicencio', 'Felix', 'felix.sueldo@centro.com.pe', '941741935', 1, 'FSUELDO', GETDATE());
go

CREATE TABLE Banco
(
IdBanco nchar(5) not null,
Codigo nvarchar(5) not null,
Banco nvarchar(50),
Abreviacion nvarchar(20),
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion date,
UsuarioEdicion nvarchar(20),
FechaEdicion date,
PRIMARY KEY(IdBanco)
);

INSERT INTO Banco (IdBanco, Codigo, Banco, Abreviacion, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('00WIE', '09', 'SCOTIABANK PERÚ', 'SCOTIABANK', 1, 'FSUELDO', GETDATE()),
('0WIE1', '09', 'SCOTIABANK PERÚ', 'SCOTIABANK', 1, 'FSUELDO', GETDATE());
go

CREATE TABLE Sociedad
(
IdSociedad nchar(4) not null,
Sociedad nvarchar(100) not null,
Abreviacion nvarchar(20),
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion date,
UsuarioEdicion nvarchar(20),
FechaEdicion date,
PRIMARY KEY(IdSociedad)
);

INSERT INTO Sociedad (IdSociedad, Sociedad, Abreviacion, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('0010', 'GLORIA S.A.', 'GLORIA', 1, 'FSUELDO', GETDATE()),
('0012', 'PANIFICADORA GLORIA S.A.', 'PANIFICADORA', 1, 'FSUELDO', GETDATE()),
('0013', 'AGROPECUARIA CHACHANI S.A.C.', 'AGROPECUARIA', 1, 'FSUELDO', GETDATE()),
('0014', 'FARMACÉUTICA DEL PACIFICO S.A.', 'FARMACÉUTICA', 1, 'FSUELDO', GETDATE()),
('0015', 'CENTRO PAPELERO S.A.C.', 'CENTRO PAPELERO', 1, 'FSUELDO', GETDATE()),
('0016', 'EMPAQ S.A.', 'EMPAQ', 1, 'FSUELDO', GETDATE()),
('0017', 'LACTEOS SAN MARTIN', 'LACTEOS SAN MARTIN', 1, 'FSUELDO', GETDATE()),
('0018', 'AGROINDUSTRIAL DEL PERU', 'AGROINDUSTRIAL', 1, 'FSUELDO', GETDATE()),
('0019', 'HOLDING ALIMENTARIO S.A.', 'HOLDING ALIMENTARIO', 1, 'FSUELDO', GETDATE()),
('0020', 'RACIEMSA S.A.', 'RACIEMSA', 1, 'FSUELDO', GETDATE()),
('0025', 'CENTRO DE SIST. Y NEG. SA', 'CENTRO', 1, 'FSUELDO', GETDATE()),
('0026', 'INV. INMOB. AREQUIPA S.A.', 'INMOB. AREQUIPA', 1, 'FSUELDO', GETDATE()),
('0030', 'YURA S.A.', 'YURA', 1, 'FSUELDO', GETDATE()),
('0034', 'EXP. FERROV. DEL PERU S.A.', 'FERROVIARIA', 1, 'FSUELDO', GETDATE()),
('0035', 'INDUSTRIAS CACHIMAYO S.A.', 'CACHIMAYO', 1, 'FSUELDO', GETDATE()),
('0036', 'CONCRETOS SUPERMIX S.A.', 'SUPERMIX', 1, 'FSUELDO', GETDATE()),
('0037', 'MAYDIRESA S.A.', 'MAYDIRESA', 1, 'FSUELDO', GETDATE()),
('0038', 'HOLDING CEMENTERO S.A.', 'HOLDING CEMENTERO', 1, 'FSUELDO', GETDATE()),
('0039', 'CONCESUR S.A.', 'CONCESUR', 1, 'FSUELDO', GETDATE()),
('0040', 'CAL & CEMENTO SUR S.A.', 'CEMENTO SUR', 1, 'FSUELDO', GETDATE()),
('0041', 'AGROJIBITO S.A.', 'AGROJIBITO', 1, 'FSUELDO', GETDATE()),
('0042', 'AGROSANJACINTO S.A.C.', 'AGROSANJACINTO', 1, 'FSUELDO', GETDATE()),
('0043', 'CASARACRA S.A.', 'CASARACRA', 1, 'FSUELDO', GETDATE()),
('0044', 'ETHANOL COMPANY S.A.', 'ETHANOL', 1, 'FSUELDO', GETDATE()),
('0045', 'LOGÍSTICA DEL PACÍFICO S.A.C.', 'DEL PACÍFICO', 1, 'FSUELDO', GETDATE()),
('0046', 'AGROLMOS S.A.', 'AGROLMOS', 1, 'FSUELDO', GETDATE()),
('0047', 'TABLEROS PERUANOS S.A.', 'TABLEROS PERUANOS', 1, 'FSUELDO', GETDATE()),
('0048', 'AGROAURORA S.A.C.', 'AGROAURORA', 1, 'FSUELDO', GETDATE()),
('0049', 'AGROINDUSTRIAS SAN JACINTO S.A.A.', 'SAN JACINTO', 1, 'FSUELDO', GETDATE()),
('0050', 'CASA GRANDE S.A.A.', 'CASA GRANDE', 1, 'FSUELDO', GETDATE()),
('0051', 'AGROCASAGRANDE S.A.C.', 'AGROCASAGRANDE', 1, 'FSUELDO', GETDATE()),
('0052', 'CARTAVIO S.A.A.', 'CARTAVIO', 1, 'FSUELDO', GETDATE()),
('0053', 'EMPRESA AGRÍCOLA SINTUCO S.A.', 'SINTUCO', 1, 'FSUELDO', GETDATE()),
('0054', 'SAN JUAN S.A.A.', 'SAN JUAN', 1, 'FSUELDO', GETDATE()),
('0055', 'MPC del Perú S.A.', 'MPC', 1, 'FSUELDO', GETDATE()),
('0057', 'TRUPAL S.A.', 'TRUPAL', 1, 'FSUELDO', GETDATE()),
('0058', 'RADIO CULTURAL S.A.C.', 'RADIO CULTURAL', 1, 'FSUELDO', GETDATE()),
('0059', 'FODINSA S.A.', 'FODINSA', 1, 'FSUELDO', GETDATE()),
('0060', 'GLORIA FOODS - JORB S.A.', 'GLORIA FOODS', 1, 'FSUELDO', GETDATE()),
('0063', 'ILLAPU ENERGY S.A.', 'ILLAPU', 1, 'FSUELDO', GETDATE()),
('0064', 'BRANDTREE GROUP S.A.', 'BRANDTREE', 1, 'FSUELDO', GETDATE()),
('0065', 'DEPRODECA S.A.C.', 'DEPRODECA', 1, 'FSUELDO', GETDATE()),
('0068', 'DISTRIBUIDORA FERIA PUCALLPA', 'FERIA PUCALLPA', 1, 'FSUELDO', GETDATE()),
('0069', 'FERIA ORIENTE S.A.C.', 'FERIA ORIENTE', 1, 'FSUELDO', GETDATE()),
('0080', 'COAZUCAR DEL PERU S.A.', 'COAZUCAR', 1, 'FSUELDO', GETDATE()),
('2000', 'COAZUCAR DEL PERU S.A.', 'COAZUCAR', 1, 'FSUELDO', GETDATE());
go

CREATE TABLE TipoOrden
(
IdBanco nchar(5) not null,
IdTipoOrden nvarchar(5) not null,
TipoOrden nvarchar(50),
Abreviacion nvarchar(20),
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion date,
UsuarioEdicion nvarchar(20),
FechaEdicion date,
PRIMARY KEY(IdBanco, IdTipoOrden)
);

ALTER TABLE TipoOrden
ADD CONSTRAINT Fk_TipoOrden_Banco FOREIGN KEY (IdBanco)
REFERENCES Banco (IdBanco);
go

INSERT INTO TipoOrden (IdBanco, IdTipoOrden, TipoOrden, Abreviacion, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('00WIE', '01', 'Pago Proveedores', 'PRO', 1, 'FSUELDO', GETDATE()),
('00WIE', '03', 'Pago Camara Comercio', 'CCE', 1, 'FSUELDO', GETDATE()),
('00WIE', '02', 'Pago Planilla', 'PLA', 1, 'FSUELDO', GETDATE()),
('00WIE', '05', 'Pago CTS', 'CTS', 1, 'FSUELDO', GETDATE()),
('00WIE', '06', 'Pago Transferencias', 'TRA', 1, 'FSUELDO', GETDATE()),
('00WIE', '07', 'Pago Transferencias BCR', 'BCR', 1, 'FSUELDO', GETDATE()),
('00WIE', '13', 'Pago Varios', 'VAR', 1, 'FSUELDO', GETDATE());
go

CREATE TABLE FormaPago
(
IdBanco nchar(5) not null,
IdFormaPago nvarchar(5) not null,
FormaPago nvarchar(50),
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion date,
UsuarioEdicion nvarchar(20),
FechaEdicion date,
PRIMARY KEY(IdBanco, IdFormaPago)
);

ALTER TABLE FormaPago
ADD CONSTRAINT Fk_FormaPago_Banco FOREIGN KEY (IdBanco)
REFERENCES Banco (IdBanco);
go

INSERT INTO FormaPago (IdBanco, IdFormaPago, FormaPago, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('00WIE', '1', 'Pago con cheque de Gerencia', 1, 'FSUELDO', GETDATE()),
('00WIE', '2', 'Abono en Cuenta Corriente', 1, 'FSUELDO', GETDATE()),
('00WIE', '3', 'Abono en Cuenta de Ahorro', 1, 'FSUELDO', GETDATE()),
('00WIE', '4', 'Abono en cuenta CCI en otro BANCO LOCAL', 1, 'FSUELDO', GETDATE());
go

CREATE TABLE TipoTransferencia
(
IdBanco nchar(5) not null,
IdTipoTransferencia nvarchar(5) not null,
TipoTransferencia nvarchar(50),
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion date,
UsuarioEdicion nvarchar(20),
FechaEdicion date,
PRIMARY KEY(IdBanco, IdTipoTransferencia)
);

ALTER TABLE TipoTransferencia
ADD CONSTRAINT Fk_TipoTransferencia_Banco FOREIGN KEY (IdBanco)
REFERENCES Banco (IdBanco);
go

INSERT INTO TipoTransferencia (IdBanco, IdTipoTransferencia, TipoTransferencia, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('00WIE', '0', 'Misma Moneda', 1, 'FSUELDO', GETDATE()),
('00WIE', '1', 'Compra M.E.  [Banco Compra]', 1, 'FSUELDO', GETDATE()),
('00WIE', '2', 'Venta M.E.  [Banco vende]', 1, 'FSUELDO', GETDATE()),
('00WIE', '3', 'BCR', 1, 'FSUELDO', GETDATE());
go

CREATE TABLE TipoCuenta
(
IdBanco nchar(5) not null,
IdTipoCuenta nvarchar(5) not null,
TipoCuenta nvarchar(50),
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion date,
UsuarioEdicion nvarchar(20),
FechaEdicion date,
PRIMARY KEY(IdBanco, IdTipoCuenta)
);

ALTER TABLE TipoCuenta
ADD CONSTRAINT Fk_TipoCuenta_Banco FOREIGN KEY (IdBanco)
REFERENCES Banco (IdBanco);
go

INSERT INTO TipoCuenta (IdBanco, IdTipoCuenta, TipoCuenta, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('00WIE', '01', 'Cuenta Corriente MN', 1, 'FSUELDO', GETDATE()),
('00WIE', '07', 'Cuenta Corriente ME', 1, 'FSUELDO', GETDATE()),
('00WIE', '14', 'Cuenta Ahorros MN', 1, 'FSUELDO', GETDATE()),
('00WIE', '83', 'Cuenta Ahorros ME', 1, 'FSUELDO', GETDATE());
go

CREATE TABLE TipoMoneda
(
IdBanco nchar(5) not null,
IdTipoMoneda nvarchar(5) not null,
TipoMoneda nvarchar(50),
Simbolo nvarchar(20),
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion date,
UsuarioEdicion nvarchar(20),
FechaEdicion date,
PRIMARY KEY(IdBanco, IdTipoMoneda)
);

ALTER TABLE TipoMoneda
ADD CONSTRAINT Fk_TipoMoneda_Banco FOREIGN KEY (IdBanco)
REFERENCES Banco (IdBanco);
go

INSERT INTO TipoMoneda (IdBanco, IdTipoMoneda, TipoMoneda, Simbolo, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('00WIE', '00', 'Soles', 'S/.', 1, 'FSUELDO', GETDATE()),
('00WIE', '01', 'Dólares', '$', 1, 'FSUELDO', GETDATE());
go

CREATE TABLE TipoDocumento
(
IdBanco nchar(5) not null,
IdTipoDocumento nvarchar(5) not null,
TipoDocumento nvarchar(50),
Abreviacion nvarchar(20),
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion date,
UsuarioEdicion nvarchar(20),
FechaEdicion date,
PRIMARY KEY(IdBanco, IdTipoDocumento)
);

ALTER TABLE TipoDocumento
ADD CONSTRAINT Fk_TipoDocumento_Banco FOREIGN KEY (IdBanco)
REFERENCES Banco (IdBanco);
go

INSERT INTO TipoDocumento (IdBanco, IdTipoDocumento, TipoDocumento, Abreviacion, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('00WIE', '1', 'Documento Nacional de Identidad', 'DNI', 1, 'FSUELDO', GETDATE()),
('00WIE', '5', 'Registro Único de Contribuyente', 'RUC', 1, 'FSUELDO', GETDATE());
go

CREATE TABLE SubTipoPago
(
IdBanco nchar(5) not null,
IdSubTipoPago nvarchar(5) not null,
SubTipoPago nvarchar(50),
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion date,
UsuarioEdicion nvarchar(20),
FechaEdicion date,
PRIMARY KEY(IdBanco, IdSubTipoPago)
);

ALTER TABLE SubTipoPago
ADD CONSTRAINT Fk_SubTipoPago_Banco FOREIGN KEY (IdBanco)
REFERENCES Banco (IdBanco);
go

INSERT INTO SubTipoPago (IdBanco, IdSubTipoPago, SubTipoPago, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('00WIE', ' ', 'Pago Normal', 1, 'FSUELDO', GETDATE()),
('00WIE', '*', 'Single Pay', 1, 'FSUELDO', GETDATE()),
('00WIE', '@', 'Pago al Exterior', 1, 'FSUELDO', GETDATE());
go

CREATE TABLE EstadoOrden
(
IdEstadoOrden nchar(2) not null,
EstadoOrden nvarchar(20) not null,
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion date,
UsuarioEdicion nvarchar(20),
FechaEdicion date,
PRIMARY KEY(IdEstadoOrden)
);

INSERT INTO EstadoOrden (IdEstadoOrden, EstadoOrden, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('PE', 'Pendiente', 1, 'FSUELDO', GETDATE()),
('LI', 'Liberado', 1, 'FSUELDO', GETDATE()),
('DE', 'Deshecho', 1, 'FSUELDO', GETDATE()),
('PP', 'Pre Aprobado', 1, 'FSUELDO', GETDATE()),
('AN', 'Anulado', 1, 'FSUELDO', GETDATE()),
('AP', 'Aprobado', 1, 'FSUELDO', GETDATE()),
('ET', 'Entregado', 1, 'FSUELDO', GETDATE()),
('PA', 'Pagado', 1, 'FSUELDO', GETDATE()),
('CE', 'Con Errores', 1, 'FSUELDO', GETDATE());
go

CREATE TABLE Trama
(
IdTrama nchar(9) not null,
IdBanco nchar(5),
Usuario nvarchar(20),
TipoOrden nchar(3),
IdSociedad nchar(4),
IdSap nvarchar(10),
Anio nchar(4),
MomentoOrden nchar(8),
IdTipoOrden nvarchar(5),
NombreArchivo nvarchar(100),
RutaArchivo nvarchar(200),
Parametros nvarchar(100),
Procesado bit,
Terminado bit,
Mensaje nvarchar(200),
UsuarioCreacion nvarchar(20),
FechaCreacion datetime,
UsuarioEdicion nvarchar(20),
FechaEdicion datetime,
PRIMARY KEY(IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden)
);
go

CREATE TABLE TramaDetalle
(
IdTrama nchar(9),
IdTramaDetalle nchar(9),
IdSociedad nchar(4),
IdSap nvarchar(10),
Anio nchar(4),
MomentoOrden nchar(8),
IdTipoOrden nvarchar(5),
Referencia2 nvarchar(50),
Cadena nvarchar(500),
Mensaje nvarchar(200),
Respuesta nvarchar(200),
UsuarioCreacion nvarchar(20),
FechaCreacion datetime,
UsuarioEdicion nvarchar(20),
FechaEdicion datetime,
PRIMARY KEY(IdTrama, IdTramaDetalle)
);

ALTER TABLE TramaDetalle
ADD CONSTRAINT Fk_Trama_TramaDetalle FOREIGN KEY (IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden)
REFERENCES Trama (IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden);
go

CREATE TABLE OrdenBancaria
(
IdOrdenBancaria nchar(9) not null,
IdBanco nchar(5) not null,
IdUsuario nchar(6) not null,
IdTipoOrden nvarchar(5) not null,
IdEstadoOrden nchar(2) not null,
IdSociedad nchar(4) not null,
IdSap nvarchar(10) not null,
Anio nchar(4) not null,
MomentoOrden nchar(8) not null,
FechaOrden date not null,
Propietario nvarchar(20) not null,
NombreArchivo nvarchar(100),
RutaArchivo nvarchar(200),
UsuarioCreacion nvarchar(20),
FechaCreacion datetime,
UsuarioEdicion nvarchar(20),
FechaEdicion datetime,
PRIMARY KEY(IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden)
);

ALTER TABLE OrdenBancaria
ADD CONSTRAINT Fk_OrdenBancaria_Banco FOREIGN KEY (IdBanco)
REFERENCES Banco (IdBanco);

ALTER TABLE OrdenBancaria
ADD CONSTRAINT Fk_OrdenBancaria_Usuario FOREIGN KEY (IdUsuario)
REFERENCES Usuario (IdUsuario);

ALTER TABLE OrdenBancaria
ADD CONSTRAINT Fk_OrdenBancaria_TipoOrden FOREIGN KEY (IdBanco, IdTipoOrden)
REFERENCES TipoOrden (IdBanco, IdTipoOrden);

ALTER TABLE OrdenBancaria
ADD CONSTRAINT Fk_OrdenBancaria_EstadoOrden FOREIGN KEY (IdEstadoOrden)
REFERENCES EstadoOrden (IdEstadoOrden);

ALTER TABLE OrdenBancaria
ADD CONSTRAINT Fk_OrdenBancaria_Sociedad FOREIGN KEY (IdSociedad)
REFERENCES Sociedad (IdSociedad);
go

CREATE TABLE OrdenBancariaDetalle
(
IdOrdenBancariaDetalle nchar(9) not null,
IdBanco nchar(5) not null,
IdSociedad nchar(4) not null,
IdSap nvarchar(10) not null,
Anio nchar(4) not null,
MomentoOrden nchar(8) not null,
IdTipoOrden nvarchar(5) not null,
IdTipoTransferencia nvarchar(5),
IdFormaPago nvarchar(5),
IdSubTipoPago nvarchar(5),
Referencia1 nvarchar(50) not null,
Referencia2 nvarchar(50),
IdMonedaCargo nvarchar(5),
CuentaCargo nvarchar(20),
MontoCargo decimal(12, 2),
IdMonedaAbono nvarchar(5),
CuentaAbono nvarchar(20),
CuentaGasto nvarchar(20),
MontoAbono decimal(12, 2),
TipoCambio decimal(6, 4),
ModuloRaiz int,
DigitoControl int,
Indicador nchar(1),
NroOperacion nvarchar(20),
Beneficiario nvarchar(80),
IdTipoDocumento nvarchar(5),
NroDocumento nvarchar(20),
Correo nvarchar(50),
NombreBanco nvarchar(50),
RucBanco nvarchar(20),
NroFactura nvarchar(20),
FechaFactura date,
FechaFinFactura date,
SignoFactura nvarchar(5),
IdRespuesta nvarchar(10),
Respuesta nvarchar(200),
NroOrden nvarchar(50),
NroConvenio nvarchar(50),
UsuarioCreacion nvarchar(20),
FechaCreacion datetime,
UsuarioEdicion nvarchar(20),
FechaEdicion datetime,
PRIMARY KEY(IdOrdenBancariaDetalle, IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden)
);

ALTER TABLE OrdenBancariaDetalle
ADD CONSTRAINT Fk_OrdenBancariaDetalle_OrdenBancaria FOREIGN KEY (IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden)
REFERENCES OrdenBancaria (IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden);

ALTER TABLE OrdenBancariaDetalle
ADD CONSTRAINT Fk_OrdenBancariaDetalle_Banco FOREIGN KEY (IdBanco)
REFERENCES Banco (IdBanco);

ALTER TABLE OrdenBancariaDetalle
ADD CONSTRAINT Fk_OrdenBancariaDetalle_Sociedad FOREIGN KEY (IdSociedad)
REFERENCES Sociedad (IdSociedad);

ALTER TABLE OrdenBancariaDetalle
ADD CONSTRAINT Fk_OrdenBancariaDetalle_TipoTransferencia FOREIGN KEY (IdBanco, IdTipoTransferencia)
REFERENCES TipoTransferencia (IdBanco, IdTipoTransferencia);

ALTER TABLE OrdenBancariaDetalle
ADD CONSTRAINT Fk_OrdenBancariaDetalle_FormaPago FOREIGN KEY (IdBanco, IdFormaPago)
REFERENCES FormaPago (IdBanco, IdFormaPago);

ALTER TABLE OrdenBancariaDetalle
ADD CONSTRAINT Fk_OrdenBancariaDetalle_SubTipoPago FOREIGN KEY (IdBanco, IdSubTipoPago)
REFERENCES SubTipoPago (IdBanco, IdSubTipoPago);

ALTER TABLE OrdenBancariaDetalle
ADD CONSTRAINT Fk_OrdenBancaria_TipoMoneda1 FOREIGN KEY (IdBanco, IdMonedaCargo)
REFERENCES TipoMoneda (IdBanco, IdTipoMoneda);

ALTER TABLE OrdenBancariaDetalle
ADD CONSTRAINT Fk_OrdenBancaria_TipoMoneda2 FOREIGN KEY (IdBanco, IdMonedaAbono)
REFERENCES TipoMoneda (IdBanco, IdTipoMoneda);

ALTER TABLE OrdenBancariaDetalle
ADD CONSTRAINT Fk_OrdenBancariaDetalle_TipoDocumento FOREIGN KEY (IdBanco, IdTipoDocumento)
REFERENCES TipoDocumento (IdBanco, IdTipoDocumento);
go

CREATE TABLE EstadoFlujo
(
IdEstadoFlujo nchar(3) not null,
EstadoFlujo nvarchar(200) not null,
Abreviacion nchar(2),
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion date,
UsuarioEdicion nvarchar(20),
FechaEdicion date,
PRIMARY KEY(IdEstadoFlujo)
);

INSERT INTO EstadoFlujo (IdEstadoFlujo, EstadoFlujo, Abreviacion, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('001', 'Se libera la orden bancaria.', 'PN', 1, 'FSUELDO', GETDATE()),
('002', 'Se deshace la orden bancaria.', 'SN', 1, 'FSUELDO', GETDATE()),
('003', 'Se pre aprueba la orden bancaria.', 'TN', 1, 'FSUELDO', GETDATE()),
('004', 'Se anula la orden bancaria.', 'CN', 1, 'FSUELDO', GETDATE()),
('005', 'Se aprueba la orden bancaria.', 'QN', 1, 'FSUELDO', GETDATE()),
('006', 'Se entrega al banco la orden bancaria.', 'SX', 1, 'FSUELDO', GETDATE()),
('007', 'Se procesa pagada la orden bancaria.', 'SP', 1, 'FSUELDO', GETDATE()),
('008', 'Se procesa con errores la orden bancaria.', 'OC', 1, 'FSUELDO', GETDATE());
go

CREATE TABLE FlujoAprobacion
(
IdFlujoAprobacion nchar(6) not null,
IdSociedad nchar(4) not null,
IdSap nvarchar(10) not null,
Anio nchar(4) not null,
MomentoOrden nchar(8) not null,
IdTipoOrden nvarchar(5) not null,
IdUsuario nchar(6) not null,
IdEstadoFlujo nchar(3) not null,
Comentarios nvarchar(300),
UsuarioCreacion nvarchar(20),
FechaCreacion datetime,
UsuarioEdicion nvarchar(20),
FechaEdicion datetime,
PRIMARY KEY(IdFlujoAprobacion, IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden)
);

ALTER TABLE FlujoAprobacion
ADD CONSTRAINT Fk_FlujoAprobacion_OrdenBancaria FOREIGN KEY (IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden)
REFERENCES OrdenBancaria (IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden);

ALTER TABLE FlujoAprobacion
ADD CONSTRAINT Fk_FlujoAprobacion_Usuario FOREIGN KEY (IdUsuario)
REFERENCES Usuario (IdUsuario);

ALTER TABLE FlujoAprobacion
ADD CONSTRAINT Fk_FlujoAprobacion_EstadoFlujo FOREIGN KEY (IdEstadoFlujo)
REFERENCES EstadoFlujo (IdEstadoFlujo);
go

CREATE TABLE Familia
(
IdFamilia nchar(3),
Descripcion varchar(100),
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion datetime,
UsuarioEdicion nvarchar(20),
FechaEdicion datetime,
PRIMARY KEY(IdFamilia)
);
go

INSERT INTO Familia (IdFamilia, Descripcion, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('001', 'Tipos de Estructura de trama.', 1, 'FSUELDO', GETDATE());

CREATE TABLE Parametro
(
IdFamilia nchar(3),
IdParametro nchar(3),
Descripcion nvarchar(100),
Abreviacion nvarchar(50),
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion datetime,
UsuarioEdicion nvarchar(20),
FechaEdicion datetime,
PRIMARY KEY(IdFamilia, IdParametro)
);

ALTER TABLE Parametro
ADD CONSTRAINT Fk_Parametro_Familia FOREIGN KEY (IdFamilia)
REFERENCES Familia (IdFamilia);
go

INSERT INTO Parametro (IdFamilia, IdParametro, Descripcion, Abreviacion, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('001', '001', 'Estrucutra de envío.', null, 1, 'FSUELDO', GETDATE()),
('001', '002', 'Estrucutra de respuesta.', null, 1, 'FSUELDO', GETDATE());
go

CREATE TABLE TramaEstructura
(
IdTramaEstructura nchar(6) not null,
Idbanco nchar(5) not null,
IdTipoOrden nvarchar(5) not null,
Estructura nvarchar(50),
Descripcion nvarchar(200),
Desde int,
Hasta int,
Longitud int,
Mandante char(1),
IdTipo nchar(3),
UsuarioCreacion nvarchar(20),
FechaCreacion datetime,
UsuarioEdicion nvarchar(20),
FechaEdicion datetime,
PRIMARY KEY(IdBanco, IdTipoOrden, Estructura)
);

ALTER TABLE TramaEstructura
ADD CONSTRAINT Fk_TramaEstructura_Banco FOREIGN KEY (IdBanco)
REFERENCES Banco (IdBanco);

ALTER TABLE TramaEstructura
ADD CONSTRAINT Fk_TramaEstructura_TipoOrden FOREIGN KEY (IdBanco, IdTipoOrden)
REFERENCES TipoOrden (IdBanco, IdTipoOrden);
go

INSERT INTO TramaEstructura (IdTramaEstructura, IdBanco, IdTipoOrden, Estructura, Descripcion, Desde, Hasta, Longitud, Mandante, IdTipo, UsuarioCreacion, FechaCreacion)
VALUES
('000001', '00WIE', '06', 'IdTipoOrden', 'Tipo de Orden', 1, 2, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000002', '00WIE', '06', 'Referencia1', 'Referencia 1', 3, 17, 15, 'M', '001', 'FSUELDO', GETDATE()),
('000003', '00WIE', '06', 'Referencia2', 'Referencia 2', 18, 33, 16, 'O', '001', 'FSUELDO', GETDATE()),
('000004', '00WIE', '06', 'IdMonedaCargo', 'Moneda del Cargo', 34, 35, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000005', '00WIE', '06', 'CuentaCargo', 'Cuenta del Cargo', 36, 55, 20, 'M', '001', 'FSUELDO', GETDATE()),
('000006', '00WIE', '06', 'FechaOrden', 'Fecha de la Orden', 56, 63, 8, 'M', '001', 'FSUELDO', GETDATE()),
('000007', '00WIE', '06', 'MontoCargo', 'Monto del Cargo', 64, 74, 11, 'M', '001', 'FSUELDO', GETDATE()),
('000008', '00WIE', '06', 'ModuloRaiz', 'Módulo Raiz', 75, 76, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000009', '00WIE', '06', 'DigitoControl', 'Dígito de Control', 77, 78, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000010', '00WIE', '06', 'IdTipoTransferencia', 'Tipo de Transferencia', 79, 79, 1, 'M', '001', 'FSUELDO', GETDATE()),
('000011', '00WIE', '06', 'Beneficiario', 'Nombre del Beneficiario', 80, 139, 60, 'M', '001', 'FSUELDO', GETDATE()),
('000012', '00WIE', '06', 'CuentaAbono', 'Cuenta de Abono', 140, 159, 20, 'M', '001', 'FSUELDO', GETDATE()),
('000013', '00WIE', '06', 'IdMonedaAbono', 'Moneda de Abono', 160, 161, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000014', '00WIE', '06', 'MontoAbono', 'Monto de Abono', 162, 172, 11, 'M', '001', 'FSUELDO', GETDATE()),
('000015', '00WIE', '06', 'Indicador', 'Indicador de Abono', 173, 173, 1, 'M', '001', 'FSUELDO', GETDATE()),
('000016', '00WIE', '06', 'TipoCambio', 'Tipo de Cambio', 174, 179, 6, 'M', '001', 'FSUELDO', GETDATE()),
('000017', '00WIE', '06', 'NroOperacion', 'Número de la Operación', 180, 187, 8, 'O', '001', 'FSUELDO', GETDATE()),
('000018', '00WIE', '06', 'IdTipoDocumento', 'Tipo Documento del Beneficiario', 188, 188, 1, 'M', '001', 'FSUELDO', GETDATE()),
('000019', '00WIE', '06', 'NroDocumento', 'Nro de Documento del Beneficiario', 189, 199, 11, 'M', '001', 'FSUELDO', GETDATE()),
('000020', '00WIE', '06', 'CodigoRespuesta', 'Código de Respuesta', 260, 262, 3, 'M', '002', 'FSUELDO', GETDATE()),
('000021', '00WIE', '06', 'DescripcionRespuesta', 'Descripción del Código de Respuesta', 263, 312, 50, 'M', '002', 'FSUELDO', GETDATE()),
('000022', '00WIE', '06', 'NumeroOrden', 'Número de la Orden', 313, 317, 5, 'M', '002', 'FSUELDO', GETDATE()),
('000023', '00WIE', '06', 'NumeroConvenio', 'Número de Convenido H2H', 318, 322, 5, 'M', '002', 'FSUELDO', GETDATE()),
('000024', '00WIE', '06', 'HoraProceso', 'Hora del proceso', 323, 330, 8, 'M', '002', 'FSUELDO', GETDATE()),

('000030', '00WIE', '07', 'IdTipoOrden', 'Tipo de Orden', 1, 2, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000031', '00WIE', '07', 'Referencia1', 'Referencia 1', 3, 17, 15, 'M', '001', 'FSUELDO', GETDATE()),
('000032', '00WIE', '07', 'Referencia2', 'Referencia 2', 18, 33, 16, 'M', '001', 'FSUELDO', GETDATE()),
('000033', '00WIE', '07', 'IdMonedaCargo', 'Moneda del Cargo', 34, 35, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000034', '00WIE', '07', 'CuentaCargo', 'Cuenta del Cargo', 36, 55, 20, 'M', '001', 'FSUELDO', GETDATE()),
('000035', '00WIE', '07', 'FechaOrden', 'Fecha de la Orden', 56, 63, 8, 'M', '001', 'FSUELDO', GETDATE()),
('000036', '00WIE', '07', 'MontoCargo', 'Monto del Cargo', 64, 74, 11, 'M', '001', 'FSUELDO', GETDATE()),
('000037', '00WIE', '07', 'ModuloRaiz', 'Módulo Raiz', 75, 76, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000038', '00WIE', '07', 'DigitoControl', 'Dígito de Control', 77, 78, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000039', '00WIE', '07', 'IdTipoTransferencia', 'Tipo de Transferencia', 79, 79, 1, 'M', '001', 'FSUELDO', GETDATE()),
('000040', '00WIE', '07', 'Beneficiario', 'Nombre del Beneficiario', 80, 139, 60, 'M', '001', 'FSUELDO', GETDATE()),
('000041', '00WIE', '07', 'CuentaAbono', 'Cuenta de Abono', 140, 159, 20, 'M', '001', 'FSUELDO', GETDATE()),
('000042', '00WIE', '07', 'CuentaGasto', 'Cuenta de Gasto', 160, 179, 20, 'M', '001', 'FSUELDO', GETDATE()),
('000043', '00WIE', '07', 'IdTipoDocumento', 'Tipo Documento del Beneficiario', 180, 180, 1, 'M', '001', 'FSUELDO', GETDATE()),
('000044', '00WIE', '07', 'NroDocumento', 'Nro de Documento del Beneficiario', 181, 191, 11, 'M', '001', 'FSUELDO', GETDATE()),
('000045', '00WIE', '07', 'RucBanco', 'RUC del otro Banco', 192, 202, 11, 'M', '001', 'FSUELDO', GETDATE()),
('000046', '00WIE', '07', 'NombreBanco', 'Nombre del otro banco', 203, 232, 30, 'M', '001', 'FSUELDO', GETDATE()),
('000047', '00WIE', '07', 'CodigoRespuesta', 'Código de Respuesta', 260, 262, 3, 'M', '002', 'FSUELDO', GETDATE()),
('000048', '00WIE', '07', 'DescripcionRespuesta', 'Descripción del Código de Respuesta', 263, 312, 50, 'M', '002', 'FSUELDO', GETDATE()),
('000049', '00WIE', '07', 'NumeroOrden', 'Número de la Orden', 313, 317, 5, 'M', '002', 'FSUELDO', GETDATE()),
('000050', '00WIE', '07', 'NumeroConvenio', 'Número de Convenido H2H', 318, 322, 5, 'M', '002', 'FSUELDO', GETDATE()),
('000051', '00WIE', '07', 'HoraProceso', 'Hora del proceso', 323, 330, 8, 'M', '002', 'FSUELDO', GETDATE()),

('000060', '00WIE', '01', 'IdTipoOrden', 'Tipo de Orden', 1, 2, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000061', '00WIE', '01', 'Referencia1', 'Referencia 1', 3, 17, 15, 'M', '001', 'FSUELDO', GETDATE()),
('000062', '00WIE', '01', 'Referencia2', 'Referencia 2', 18, 33, 16, 'O', '001', 'FSUELDO', GETDATE()),
('000063', '00WIE', '01', 'IdMonedaAbono', 'Moneda de Pago', 34, 35, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000064', '00WIE', '01', 'CuentaCargo', 'Cuenta del Cargo', 36, 55, 20, 'M', '001', 'FSUELDO', GETDATE()),
('000065', '00WIE', '01', 'FechaOrden', 'Fecha de la Orden', 56, 63, 8, 'M', '001', 'FSUELDO', GETDATE()),
('000066', '00WIE', '01', 'NroDocumento', 'RUC del proveedor', 64, 74, 11, 'M', '001', 'FSUELDO', GETDATE()),
('000067', '00WIE', '01', 'Beneficiario', 'Nombre del Proveedor', 75, 134, 60, 'M', '001', 'FSUELDO', GETDATE()),
('000068', '00WIE', '01', 'IdFormaPago', 'Forma de Pago', 135, 135, 1, 'M', '001', 'FSUELDO', GETDATE()),
('000069', '00WIE', '01', 'CuentaAbono', 'Cuenta a Abonar', 136, 155, 20, 'M', '001', 'FSUELDO', GETDATE()),
('000070', '00WIE', '01', 'FechaFactura', 'Fecha de la Factura', 156, 163, 8, 'M', '001', 'FSUELDO', GETDATE()),
('000071', '00WIE', '01', 'FechaFinFactura', '', 164, 171, 8, 'M', '001', 'FSUELDO', GETDATE()),
('000072', '00WIE', '01', 'NroFactura', 'Número Factura', 172, 191, 20, 'M', '001', 'FSUELDO', GETDATE()),
('000073', '00WIE', '01', 'MontoAbono', 'Importe Neto', 192, 202, 11, 'M', '001', 'FSUELDO', GETDATE()),
('000074', '00WIE', '01', 'ModuloRaiz', 'Módulo Raiz', 203, 204, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000075', '00WIE', '01', 'DigitoControl', 'Dígito de Control', 205, 206, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000076', '00WIE', '01', 'IdSubTipoPago', 'Sub Tipo de Pago', 207, 207, 1, 'M', '001', 'FSUELDO', GETDATE()),
('000077', '00WIE', '01', 'SignoFactura', 'Signo Factura', 208, 208, 1, 'M', '001', 'FSUELDO', GETDATE()),
('000078', '00WIE', '01', 'Correo', 'Email Proveedor', 209, 258, 50, 'O', '001', 'FSUELDO', GETDATE()),
('000079', '00WIE', '01', 'CodigoRespuesta', 'Código de Respuesta', 260, 262, 3, 'M', '002', 'FSUELDO', GETDATE()),
('000080', '00WIE', '01', 'DescripcionRespuesta', 'Descripción del Código de Respuesta', 263, 312, 50, 'M', '002', 'FSUELDO', GETDATE()),
('000081', '00WIE', '01', 'NumeroOrden', 'Número de la Orden', 313, 317, 5, 'M', '002', 'FSUELDO', GETDATE()),
('000082', '00WIE', '01', 'NumeroConvenio', 'Número de Convenido H2H', 318, 322, 5, 'M', '002', 'FSUELDO', GETDATE()),
('000083', '00WIE', '01', 'HoraProceso', 'Hora del proceso', 323, 330, 8, 'M', '002', 'FSUELDO', GETDATE()),

('000090', '00WIE', '03', 'IdTipoOrden', 'Tipo de Orden', 1, 2, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000091', '00WIE', '03', 'Referencia1', 'Referencia 1', 3, 17, 15, 'M', '001', 'FSUELDO', GETDATE()),
('000092', '00WIE', '03', 'Referencia2', 'Referencia 2', 18, 33, 16, 'O', '001', 'FSUELDO', GETDATE()),
('000093', '00WIE', '03', 'IdMonedaAbono', 'Moneda de Pago', 34, 35, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000094', '00WIE', '03', 'CuentaCargo', 'Cuenta del Cargo', 36, 55, 20, 'M', '001', 'FSUELDO', GETDATE()),
('000095', '00WIE', '03', 'FechaOrden', 'Fecha de la Orden', 56, 63, 8, 'M', '001', 'FSUELDO', GETDATE()),
('000096', '00WIE', '03', 'NroDocumento', 'RUC del proveedor', 64, 74, 11, 'M', '001', 'FSUELDO', GETDATE()),
('000097', '00WIE', '03', 'Beneficiario', 'Nombre del Proveedor', 75, 134, 60, 'M', '001', 'FSUELDO', GETDATE()),
('000098', '00WIE', '03', 'IdFormaPago', 'Forma de Pago', 135, 135, 1, 'M', '001', 'FSUELDO', GETDATE()),
('000099', '00WIE', '03', 'CuentaAbono', 'Cuenta a Abonar', 136, 155, 20, 'M', '001', 'FSUELDO', GETDATE()),
('000100', '00WIE', '03', 'FechaFactura', 'Fecha de la Factura', 156, 163, 8, 'M', '001', 'FSUELDO', GETDATE()),
('000101', '00WIE', '03', 'FechaFinFactura', '', 164, 171, 8, 'M', '001', 'FSUELDO', GETDATE()),
('000102', '00WIE', '03', 'NroFactura', 'Número Factura', 172, 191, 20, 'M', '001', 'FSUELDO', GETDATE()),
('000103', '00WIE', '03', 'MontoAbono', 'Importe Neto', 192, 202, 11, 'M', '001', 'FSUELDO', GETDATE()),
('000104', '00WIE', '03', 'ModuloRaiz', 'Módulo Raiz', 203, 204, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000105', '00WIE', '03', 'DigitoControl', 'Dígito de Control', 205, 206, 2, 'M', '001', 'FSUELDO', GETDATE()),
('000106', '00WIE', '03', 'IdSubTipoPago', 'Sub Tipo de Pago', 207, 207, 1, 'M', '001', 'FSUELDO', GETDATE()),
('000107', '00WIE', '03', 'SignoFactura', 'Signo Factura', 208, 208, 1, 'M', '001', 'FSUELDO', GETDATE()),
('000108', '00WIE', '03', 'Correo', 'Email Proveedor', 209, 258, 50, 'O', '001', 'FSUELDO', GETDATE()),
('000109', '00WIE', '03', 'CodigoRespuesta', 'Código de Respuesta', 260, 262, 3, 'M', '002', 'FSUELDO', GETDATE()),
('000110', '00WIE', '03', 'DescripcionRespuesta', 'Descripción del Código de Respuesta', 263, 312, 50, 'M', '002', 'FSUELDO', GETDATE()),
('000111', '00WIE', '03', 'NumeroOrden', 'Número de la Orden', 313, 317, 5, 'M', '002', 'FSUELDO', GETDATE()),
('000112', '00WIE', '03', 'NumeroConvenio', 'Número de Convenido H2H', 318, 322, 5, 'M', '002', 'FSUELDO', GETDATE()),
('000113', '00WIE', '03', 'HoraProceso', 'Hora del proceso', 323, 330, 8, 'M', '002', 'FSUELDO', GETDATE());
go

CREATE TABLE Token
(
IdToken nchar(6),
IdUsuario nchar(6) not null,
Token nvarchar(255) not null,
Fecha date not null,
UsuarioCreacion nvarchar(20),
FechaCreacion datetime,
UsuarioEdicion nvarchar(20),
FechaEdicion datetime
PRIMARY KEY(IdToken)
)
go

CREATE TABLE CorreoEstructura
(
IdCorreoEstructura nchar(6),
Descripcion nvarchar(100),
Asunto nvarchar(200),
Cuerpo nvarchar(max),
Estado bit,
UsuarioCreacion nvarchar(20),
FechaCreacion datetime,
UsuarioEdicion nvarchar(20),
FechaEdicion datetime
PRIMARY KEY(IdCorreoEstructura)
);

INSERT INTO CorreoEstructura (IdCorreoEstructura, Descripcion, Asunto, Cuerpo, Estado, UsuarioCreacion, FechaCreacion)
VALUES
('000001', 'Correo para envío de código token para inicio de sesión en monitor no sap.', 'Código de autorización para monitor no sap [Host To Host]', 
'<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
	<title>Monitor No Sap</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0"/>
	<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/css/bootstrap.min.css" integrity="sha384-GJzZqFGwb1QTTN6wy59ffF1BuGJpLSa9DkKMp0DgiMDm4iYMj70gZWKYbI706tWS" crossorigin="anonymous">
	<style>
	html {
		font-family: sans-serif;
		line-height: 1.15;
		-webkit-text-size-adjust: 100%;
		-ms-text-size-adjust: 100%;
		-ms-overflow-style: scrollbar;
		-webkit-tap-highlight-color: rgba(0, 0, 0, 0);
	}
	body {
		margin: 0;
		font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif, "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol", "Noto Color Emoji";
		font-size: 1rem;
		font-weight: 400;
		line-height: 1.5;
		color: #212529;
		text-align: left;
		background-color: #fff;
	}
	button {
		border-radius: 0;
	}
	button:focus {
		outline: 1px dotted;
		outline: 5px auto -webkit-focus-ring-color;
	}
	input,
	button,
	select,
	optgroup,
	textarea {
		margin: 0;
		font-family: inherit;
		font-size: inherit;
		line-height: inherit;
	}
	button,
	input {
		overflow: visible;
	}
	button,
	select {
		text-transform: none;
	}
	button,
	html [type="button"],
	[type="reset"],
	[type="submit"] {
		-webkit-appearance: button;
	}
	button::-moz-focus-inner,
	[type="button"]::-moz-focus-inner,
	[type="reset"]::-moz-focus-inner,
	[type="submit"]::-moz-focus-inner {
		padding: 0;
		border-style: none;
	}
	a.bg-primary:hover, a.bg-primary:focus,
	button.bg-primary:hover,
	button.bg-primary:focus {
		background-color: #0062cc !important;
	}
	.jumbotron {
		padding: 2rem 1rem;
		margin-bottom: 2rem;
		background-color: #e9ecef;
		border-radius: 0.3rem;
	}
	.display-4 {
		font-size: 3.5rem;
		font-weight: 300;
		line-height: 1.2;
	}
	.lead {
		font-size: 1.25rem;
		font-weight: 300;
	}
	.my-4 {
		margin-bottom: 1.5rem !important;
	}
	.btn {
		display: inline-block;
		font-weight: 400;
		text-align: center;
		white-space: nowrap;
		vertical-align: middle;
		-webkit-user-select: none;
		-moz-user-select: none;
		-ms-user-select: none;
		user-select: none;
		border: 1px solid transparent;
		padding: 0.375rem 0.75rem;
		font-size: 1rem;
		line-height: 1.5;
		border-radius: 0.25rem;
		transition: color 0.15s ease-in-out, background-color 0.15s ease-in-out, border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
	}
	.btn-primary {
		color: #fff;
		background-color: #007bff;
		border-color: #007bff;
	}
	.btn-primary:hover {
		color: #fff;
		background-color: #0069d9;
		border-color: #0062cc;
	}
	</style>
</head>
  <body>
    <div class="jumbotron">
      <h1 class="display-4">Hola!</h1>
      <p class="lead">Este es un correo creado y enviado de manera automaticamente por el sistema a solicitud suya.</p>
      <p class="lead">Solicitud de codigo token para el inicio de sesión en el sistema host to host no sap.</p>
      <hr class="my-4">
      <p>La duración del código token es de 1 día, es decir solo es válido para el día en curso.</p>
      <p>A continuación se detalla el código solicitado.</p>
      <p>Código token: <strong>[token]</strong></p>
      <a class="btn btn-primary btn-lg" href="http://10.20.20.104" role="button">Iniciar Sesión</a>
    </div>
  </body>
</html>', 1, 'FSUELDO', GETDATE()),
('000002', 'Correo para envío de enlace para recuperación de contraseña del monitor no sap', 'Código de verificación para monitor no sap [Host To Host]', 
'<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
	<title>Monitor No Sap</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0"/>
	<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/css/bootstrap.min.css" integrity="sha384-GJzZqFGwb1QTTN6wy59ffF1BuGJpLSa9DkKMp0DgiMDm4iYMj70gZWKYbI706tWS" crossorigin="anonymous">
	<style>
	html {
		font-family: sans-serif;
		line-height: 1.15;
		-webkit-text-size-adjust: 100%;
		-ms-text-size-adjust: 100%;
		-ms-overflow-style: scrollbar;
		-webkit-tap-highlight-color: rgba(0, 0, 0, 0);
	}
	body {
		margin: 0;
		font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif, "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol", "Noto Color Emoji";
		font-size: 1rem;
		font-weight: 400;
		line-height: 1.5;
		color: #212529;
		text-align: left;
		background-color: #fff;
	}
	button {
		border-radius: 0;
	}
	button:focus {
		outline: 1px dotted;
		outline: 5px auto -webkit-focus-ring-color;
	}
	input,
	button,
	select,
	optgroup,
	textarea {
		margin: 0;
		font-family: inherit;
		font-size: inherit;
		line-height: inherit;
	}
	button,
	input {
		overflow: visible;
	}
	button,
	select {
		text-transform: none;
	}
	button,
	html [type="button"],
	[type="reset"],
	[type="submit"] {
		-webkit-appearance: button;
	}
	button::-moz-focus-inner,
	[type="button"]::-moz-focus-inner,
	[type="reset"]::-moz-focus-inner,
	[type="submit"]::-moz-focus-inner {
		padding: 0;
		border-style: none;
	}
	a.bg-primary:hover, a.bg-primary:focus,
	button.bg-primary:hover,
	button.bg-primary:focus {
		background-color: #0062cc !important;
	}
	.jumbotron {
		padding: 2rem 1rem;
		margin-bottom: 2rem;
		background-color: #e9ecef;
		border-radius: 0.3rem;
	}
	.display-4 {
		font-size: 3.5rem;
		font-weight: 300;
		line-height: 1.2;
	}
	.lead {
		font-size: 1.25rem;
		font-weight: 300;
	}
	.my-4 {
		margin-bottom: 1.5rem !important;
	}
	.btn {
		display: inline-block;
		font-weight: 400;
		text-align: center;
		white-space: nowrap;
		vertical-align: middle;
		-webkit-user-select: none;
		-moz-user-select: none;
		-ms-user-select: none;
		user-select: none;
		border: 1px solid transparent;
		padding: 0.375rem 0.75rem;
		font-size: 1rem;
		line-height: 1.5;
		border-radius: 0.25rem;
		transition: color 0.15s ease-in-out, background-color 0.15s ease-in-out, border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
	}
	.btn-primary {
		color: #fff;
		background-color: #007bff;
		border-color: #007bff;
	}
	.btn-primary:hover {
		color: #fff;
		background-color: #0069d9;
		border-color: #0062cc;
	}
	</style>
</head>
  <body>
    <div class="jumbotron">
      <h1 class="display-4">Hola!</h1>
      <p class="lead">Este es un correo creado y enviado de manera automaticamente por el sistema a solicitud suya.</p>
      <p class="lead">Solicitud de recuperación de contraseña del sistema host to host no sap.</p>
      <hr class="my-4">
      <p>A continuación siga las instrucciones en el siguiente enlace.</p>
      <p>Enlace: <a class=''btn btn-primary'' href=''[enlace]''>Click Aqui!</a></p>
    </div>
  </body>
</html>', 1, 'FSUELDO', GETDATE());
go