
CREATE PROCEDURE spw_hth_crearUsuario
/*************************************************************************
Nombre: spw_hth_crearUsuario
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_crearUsuario 'nombres', 'apePaterno', 'apeMaterno', 'correo', 'celular', 'usuarioCreacion';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@nombres nvarchar(50),
@apePaterno nvarchar(50),
@apeMaterno nvarchar(50),
@correo nvarchar(50),
@celular nvarchar(15),
@idUsuario nchar(6) OUT
)
AS
BEGIN TRY
SET NOCOUNT ON;

declare
	@correlativo int = 0,
	@codigo int = 0,
	@mensaje nvarchar(200) = '',
	@estadoActivo bit = 1,
	@usuarioHostToHost nvarchar(20) = 'Web HostToHost';

BEGIN TRANSACTION

SELECT @correlativo = MAX(IdUsuario) FROM dbo.Usuario;
SET @correlativo = @correlativo + 1;
SELECT @idUsuario = RIGHT(REPLICATE('0', 6) + CAST(@correlativo as nvarchar), 6);

INSERT INTO
	dbo.Usuario
	(
	IdUsuario,
	ApePaterno,
	ApeMaterno,
	Nombres,
	Correo,
	Celular,
	Estado,
	UsuarioCreacion,
	FechaCreacion
	)
	VALUES
	(
	@idUsuario,
	@apePaterno,
	@apeMaterno,
	@nombres,
	@correo,
	@celular,
	@estadoActivo,
	@usuarioHostToHost,
	GETDATE()
	);

COMMIT TRANSACTION

SET @codigo = 10;
SET @mensaje = 'Se creó satisfactoriamente el usuario.';

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION
print 'Error Código: ' + CAST(ERROR_NUMBER() as nvarchar)
print 'Error Línea: ' + CAST(ERROR_LINE() as nvarchar);
print 'Error Mensaje: ' + ERROR_MESSAGE();
SELECT @codigo = -15, @mensaje = ERROR_MESSAGE(); 
SELECT @codigo as codigo, @mensaje as mensaje;

END CATCH
SELECT @codigo as codigo, @mensaje as mensaje;
go

CREATE PROCEDURE spw_hth_eliminarUsuario
/*************************************************************************
Nombre: spw_hth_eliminarUsuario
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_eliminarUsuario '000002';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6)
)
AS
BEGIN TRY
SET NOCOUNT ON;

declare @codigo int = 0,
		@mensaje nvarchar(200) = '';

BEGIN TRANSACTION

DELETE FROM
	dbo.Usuario
WHERE
	IdUsuario = @idUsuario and Estado = 1;

COMMIT TRANSACTION

SELECT @codigo = 10, @mensaje = 'Se eliminó satisfactoriamente el usuario.';

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION
print 'Error Código: ' + CAST(ERROR_NUMBER() as nvarchar)
print 'Error Línea: ' + CAST(ERROR_LINE() as nvarchar);
print 'Error Mensaje: ' + ERROR_MESSAGE();
SELECT @codigo = -15, @mensaje = ERROR_MESSAGE();

END CATCH
SELECT @codigo as codigo, @mensaje as mensaje;
go

CREATE PROCEDURE spw_hth_consultarUsuario
/*************************************************************************
Nombre: spw_hth_consultarUsuario
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_consultarUsuario 'fsueldo';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@usuario nvarchar(20)
)
AS
BEGIN
SET NOCOUNT ON;

SELECT
	u.IdUsuario,
	au.UserName,
	u.ApePaterno,
	u.ApeMaterno,
	u.Nombres,
	u.Correo,
	u.Celular,
	CONVERT(nvarchar(10), COALESCE(u.FechaCreacion, ''), 103) as FechaCreacion
FROM
	dbo.Usuario u
	join dbo.AspNetUsers au on u.IdUsuario = au.IdUsuario
WHERE
	u.Estado = 1 and au.UserName = @usuario;

END
go

CREATE PROCEDURE sps_hth_procesarTrama
/*************************************************************************
Nombre: sps_hth_procesarTrama
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec sps_hth_procesarTrama '00WIE', 'WS_SULCA', 'TRA', '0010', '1234567890', '2018', '20181225', 'NombreArchivo.txt',
'Ruta Archivo', 'Parametros',
N'<?xml version="1.0" encoding="utf-16"?>
<tramas>
	<trama><id>1</id><cadena>06120000000100004CS_HMONTILLA    00428424000001        201811290000019670090073LECHE GLORIA S.A.                                           002193000100146075150084240000010131416152010019579720100047</cadena></trama>
	<trama><id>6</id><cadena>06120000000100004CS_HMONTILLA    00428424000001        201811290000019670090073LECHE GLORIA S.A.                                           002193000100146075150084240000010131416152010019579720100047</cadena></trama>
</tramas>';
exec sps_hth_procesarTrama '00WIE', 'WS_SULCA', 'BCR', '0010', '1234567890', '2018', '20181224', 'NombreArchivo.txt',
'Ruta Archivo', 'Parametros',
N'<?xml version="1.0" encoding="utf-16"?>
<tramas>
	<trama><id>1</id><cadena>07120000000100004CS_HMONTILLA    00428424000001        201811290000019670090073LECHE GLORIA S.A.                                           002193000100146075100219300010014607515510842400000152010019579BANCO DE CREDITO              </cadena></trama>
	<trama><id>6</id><cadena>07120000000100004CS_HMONTILLA    00428424000001        201811290000019670090073LECHE GLORIA S.A.                                           002193000100146075100219300010014607515510842400000152010019579BANCO DE CREDITO              </cadena></trama>
</tramas>';
exec sps_hth_procesarTrama '00WIE', 'WS_SULCA', 'PRO', '0010', '1234567890', '2018', '20181223', 'NombreArchivo.txt',
'Ruta Archivo', 'Parametros',
N'<?xml version="1.0" encoding="utf-16"?>
<tramas>
	<trama><id>1</id><cadena>01120000000100004CS_HMONTILLA    00428424000001        2018112934196790073LECHE GLORIA S.A.                                           421930001001460751500201804022018043000000152010019579720009100047215099 +CORREOPROVEEDOR@PROVEEDOR.COM                     </cadena></trama>
	<trama><id>6</id><cadena>01120000000100004CS_HMONTILLA    00428424000001        2018112934196790073LECHE GLORIA S.A.                                           421930001001460751500201804022018043000000152010019579720009100047215099 +CORREOPROVEEDOR@PROVEEDOR.COM                     </cadena></trama>
</tramas>';
<trama><id>7</id><cadena>9900000600000000943900020160401000166</cadena></trama>

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idBanco nchar(5),
@usuario nvarchar(20),
@tipoOrden nchar(3),
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@propietario nvarchar(20),
@nombreArchivo nvarchar(100),
@rutaArchivo nvarchar(200),
@parametros nvarchar(100),
@tramaDetalle xml
)
AS
BEGIN TRY
SET NOCOUNT ON;

declare
	@idTrama nchar(9) = '',
	@codigo int = 0,
	@mensaje nvarchar(200) = '',
	@correlativoOrdenBancaria int = 0,
	@correlativoTrama int = 0,
	@correlativoTramaDetalle int = 0,
	@correlativoFlujoAprobacion int = 0,
	@cantidadLineas int = 0,
	@sinProcesar bit = 0,
	@conProcesar bit = 1,
	@usuarioHostToHost nvarchar(20) = 'Servicio Windows',
	@delimitador nchar(1) = '|',
	@contador int = 1,
	@contadorTotal int = 0,
	@idTramaDetalle nchar(9) = '',
	@cadena nvarchar(500) = '',
	@idOrdenBancaria nchar(9) = '',
	@idTipoOrden nvarchar(5),
	@tipoOrdenTransferencia nchar(3) = 'TRA',
	@tipoOrdenTransferenciaBcr nchar(3) = 'BCR',
	@tipoOrdenProveedor nchar(3) = 'PRO',
	@tipoOrdenCamaraComercio nchar(3) = 'CCE',
	@idTipoOrdenTransferencia nvarchar(5) = '06',
	@idTipoOrdenTransferenciaBcr nvarchar(5) = '07',
	@idTipoOrdenProveedor nvarchar(5) = '01',
	@idTipoOrdenCamaraComercio nvarchar(5) = '03',
	@esFechaOrden bit = 0,
	@fechaOrden date,
	@idUsuario nchar(6) = '',
	@idEstadoOrdenLiberado nchar(2) = 'LI',
	@contadorTrama int = 0,
	@codigo_ok int = 10,
	@idFlujoAprobacion nchar(6) = '',
	@idEstadoFlujoLiberado nchar(3) = '001',
	@desdeReferencia1 int = 0,
	@longitudReferencia1 int = 0,
	@idTipoEnvio nchar(3) = '001',
	@REFERENCIA_1 nvarchar(50) = 'Referencia1',
	@referencia1 nvarchar(50) = '';

declare @trama as table
(
id int,
cadena nvarchar(500)
);

BEGIN TRANSACTION

INSERT INTO @trama
SELECT
	trama.col.value('id[1]', 'int') id,
	trama.col.value('cadena[1]', 'nvarchar(500)') cadena
FROM
	@tramaDetalle.nodes('/tramas/trama') trama(col);

SELECT @cantidadLineas = COUNT(cadena) FROM @trama;
SELECT @idTipoOrden = CASE @tipoOrden WHEN @tipoOrdenTransferencia THEN @idTipoOrdenTransferencia WHEN @tipoOrdenTransferenciaBcr THEN @idTipoOrdenTransferenciaBcr WHEN @tipoOrdenProveedor THEN @idTipoOrdenProveedor WHEN @tipoOrdenCamaraComercio THEN @idTipoOrdenCamaraComercio ELSE '' END;
SELECT @esFechaOrden = dbo.fn_hth_verificarFecha(@momentoOrden);
SELECT @fechaOrden = CASE @esFechaOrden WHEN 0 THEN null ELSE CAST(@momentoOrden as date) END;

SELECT
	@idUsuario = u.IdUsuario
FROM
	dbo.AspNetUsers au
	join Usuario u on au.IdUsuario = u.IdUsuario and u.Estado = 1
WHERE
	au.UserName = LOWER(@usuario);

SELECT @idUsuario = CASE @idUsuario WHEN '' THEN '000001' ELSE @idUsuario END;

if (@idBanco = '')
	SET @mensaje = @mensaje + 'Código del banco no presente.' + @delimitador;
if (@usuario = '')
	SET @mensaje = @mensaje + 'Usuario liberador no presente.' + @delimitador;
if (@tipoOrden = '')
	SET @mensaje = @mensaje + 'Tipo de orden abreviación no presente.' + @delimitador;
if (@idSociedad= '')
	SET @mensaje = @mensaje + 'Código de sociedad no presente.' + @delimitador;
if (@idSap = '')
	SET @mensaje = @mensaje + 'Código SAP no presente.' + @delimitador;
if (@anio = '')
	SET @mensaje = @mensaje + 'Año contable no presente.' + @delimitador;
if (@momentoOrden = '')
	SET @mensaje = @mensaje + 'Fecha de orden no presente.' + @delimitador;
else
	SELECT @mensaje = CASE @esFechaOrden WHEN 0 THEN @mensaje + 'La fecha de orden posee un formato incorrecto.' + @delimitador ELSE @mensaje END;
if (@nombreArchivo = '')
	SET @mensaje = @mensaje + 'Nombre del archivo no presente.' + @delimitador;
if (@rutaArchivo = '')
	SET @mensaje = @mensaje + 'Ruta del archivo no presente.' + @delimitador;
if (@parametros = '')
	SET @mensaje = @mensaje + 'Parámetros no presentes.' + @delimitador;
if (@cantidadLineas = 0)
	SET @mensaje = @mensaje + 'Contenido del archivo vacío.' + @delimitador;

SELECT @mensaje = CASE @mensaje WHEN '' THEN null ELSE @mensaje END;
SELECT @correlativoTrama = ISNULL(MAX(IdTrama), 0) FROM dbo.Trama;
SELECT @correlativoTrama = @correlativoTrama + 1;
SET @idTrama = RIGHT(REPLICATE('0', 9) + CAST(@correlativoTrama as nvarchar), 9);

INSERT INTO
	Trama
	(
	IdTrama,
	IdBanco,
	Usuario,
	TipoOrden,
	IdSociedad,
	IdSap,
	Anio,
	MomentoOrden,
	IdTipoOrden,
	NombreArchivo,
	RutaArchivo,
	Parametros,
	Procesado,
	Mensaje,
	UsuarioCreacion,
	FechaCreacion
	)
VALUES
	(
	@idTrama,
	@idBanco,
	@usuario,
	@tipoOrden,
	@idSociedad,
	@idSap,
	@anio,
	@momentoOrden,
	@idTipoOrden,
	@nombreArchivo,
	@rutaArchivo,
	@parametros,
	@sinProcesar,
	@mensaje,
	@usuarioHostToHost,
	GETDATE()
	);

SELECT @correlativoOrdenBancaria = ISNULL(MAX(IdOrdenBancaria), 0) FROM dbo.OrdenBancaria;
SELECT @correlativoOrdenBancaria = @correlativoOrdenBancaria + 1;
SELECT @idOrdenBancaria = RIGHT(REPLICATE('0', 9) + CAST(@correlativoOrdenBancaria as nvarchar), 9);

INSERT INTO
	dbo.OrdenBancaria
	(
	IdOrdenBancaria,
	IdBanco,
	IdUsuario,
	IdTipoOrden,
	IdEstadoOrden,
	IdSociedad,
	IdSap,
	Anio,
	MomentoOrden,
	FechaOrden,
	Propietario,
	NombreArchivo,
	RutaArchivo,
	UsuarioCreacion,
	FechaCreacion
	)
VALUES
	(
	@idOrdenBancaria,
	@idBanco,
	@idUsuario,
	@idTipoOrden,
	@idEstadoOrdenLiberado,
	@idSociedad,
	@idSap,
	@anio,
	@momentoOrden,
	@fechaOrden,
	@propietario,
	@nombreArchivo,
	@rutaArchivo,
	@usuarioHostToHost,
	GETDATE()
	);

SELECT @desdeReferencia1 = Desde, @longitudReferencia1 = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @REFERENCIA_1;
SET @contadorTotal = @cantidadLineas;

WHILE (@contador <= @contadorTotal)
BEGIN
	SELECT @correlativoTramaDetalle = ISNULL(MAX(IdTramaDetalle), 0) FROM dbo.TramaDetalle WHERE idTrama = @idTrama;
	SELECT @correlativoTramaDetalle = @correlativoTramaDetalle + 1;
	SET @idTramaDetalle = RIGHT(REPLICATE('0', 9) + CAST(@correlativoTramaDetalle as nvarchar), 9);
	SELECT @cadena = cadena FROM @trama WHERE id = @contador;
	SELECT @referencia1 = SUBSTRING(@cadena, @desdeReferencia1, @longitudReferencia1);

	INSERT INTO
		dbo.TramaDetalle
		(
		IdTrama,
		IdTramaDetalle,
		IdSociedad,
		IdSap,
		Anio,
		MomentoOrden,
		IdTipoOrden,
		Referencia2,
		Cadena,
		UsuarioCreacion,
		FechaCreacion
		)
	VALUES
		(
		@idTrama,
		@idTramaDetalle,
		@idSociedad,
		@idSap,
		@anio,
		@momentoOrden,
		@idTipoOrden,
		@referencia1,
		LTRIM(RTRIM(@cadena)),
		@usuarioHostToHost,
		GETDATE()
		);

	EXEC @codigo = dbo.sps_hth_descomponerTrama @idBanco, @idTipoOrden, @idSociedad, @idSap, @anio, @momentoOrden, @cadena, @mensaje OUTPUT;

	IF (@codigo = @codigo_ok)
	BEGIN
		SET @contadorTrama = @contadorTrama + 1;
	END
	ELSE
	BEGIN
		--UPDATE dbo.TramaDetalle SET Mensaje = @mensaje WHERE idTramaDetalle = @idTramaDetalle;
		RAISERROR(@mensaje, 16, 1);
	END

	SELECT @correlativoTramaDetalle = 0, @idTramaDetalle = '', @cadena = '', @referencia1 = '', @codigo = 0, @mensaje = '';
	SET @contador = @contador + 1;
END

IF (@contadorTrama = @contadorTotal)
BEGIN
	UPDATE
		dbo.Trama
	SET
		Procesado = @conProcesar
	WHERE
		IdTrama = @idTrama;

	SELECT @correlativoFlujoAprobacion = ISNULL(MAX(IdFlujoAprobacion), 0) FROM dbo.FlujoAprobacion;
	SELECT @correlativoFlujoAprobacion = @correlativoFlujoAprobacion + 1;
	SELECT @idFlujoAprobacion = RIGHT(REPLICATE('0', 6) + CAST(@correlativoFlujoAprobacion as nvarchar), 6);
	
	INSERT INTO
		dbo.FlujoAprobacion
		(
		IdFlujoAprobacion,
		IdSociedad,
		IdSap,
		Anio,
		MomentoOrden,
		IdTipoOrden,
		IdUsuario,
		IdEstadoFlujo,
		Comentarios,
		UsuarioCreacion,
		FechaCreacion
		)
	VALUES
		(
		@idFlujoAprobacion,
		@idSociedad,
		@idSap,
		@anio,
		@momentoOrden,
		@idTipoOrden,
		@idUsuario,
		@idEstadoFlujoLiberado,
		null,
		@usuarioHostToHost,
		GETDATE()
		);

END

IF (@@TRANCOUNT > 0)
BEGIN
	COMMIT TRANSACTION
END

SELECT @codigo = 10, @mensaje = 'Se registró satisfactoriamente la trama y trama detalle.';

END TRY
BEGIN CATCH

IF (@@TRANCOUNT > 0)
BEGIN
	ROLLBACK TRANSACTION
END
print 'Error Código: ' + CAST(ERROR_NUMBER() as nvarchar)
print 'Error Línea: ' + CAST(ERROR_LINE() as nvarchar);
print 'Error Mensaje: ' + ERROR_MESSAGE();
SELECT @codigo = -15, @mensaje = ERROR_MESSAGE(); 

END CATCH
SELECT @codigo as codigo, @mensaje as mensaje;
go

CREATE PROCEDURE sps_hth_descomponerTrama
/*************************************************************************
Nombre: sps_hth_descomponerTrama
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec sps_hth_descomponerTrama '00WIE', '06', '0010', '1234567890', '2018', '20181220',
'06120000000100004CS_HMONTILLA    00428424000001        201811290000019670090073LECHE GLORIA S.A.                                           002193000100146075150084240000010131416152010019579720100047';
exec sps_hth_descomponerTrama '00WIE', '07', '0010', '1234567890', '2018', '20181220',
'07120000000100004CS_HMONTILLA    00428424000001        201811290000019670090073LECHE GLORIA S.A.                                           002193000100146075100219300010014607515510842400000152010019579BANCO DE CREDITO              ';
exec sps_hth_descomponerTrama '00WIE', '01', '0010', '1234567890', '2018', '20181220',
'01120000000100004CS_HMONTILLA    00428424000001        2018112934196790073LECHE GLORIA S.A.                                           421930001001460751500201804022018043000000152010019579720009100047215099 +CORREOPROVEEDOR@PROVEEDOR.COM                     ';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idBanco nchar(5),
@idTipoOrden nvarchar(5),
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@cadena nvarchar(500),
@mensaje nvarchar(200) = '' OUTPUT
)
AS
BEGIN TRY
SET NOCOUNT ON;

declare
	@codigo int = 0,
	@desde int = 0,
	@longitud int = 0,
	@correlativoOrdenBancariaDetalle int = 0,
	@idOrdenBancariaDetalle nchar(9) = '',
	@idTipoOrdenTransferencia nvarchar(5) = '06',
	@idTipoOrdenTransferenciaBcr nvarchar(5) = '07',
	@idTipoOrdenProveedor nvarchar(5) = '01',
	@idTipoOrdenCamaraComercio nvarchar(5) = '03',
	@idTipoEnvio nchar(3) = '001',
	@ID_TIPO_TRANSFERENCIA nvarchar(50) = 'IdTipoTransferencia',
	@ID_FORMA_PAGO nvarchar(50) = 'IdFormaPago',
	@ID_SUBTIPO_PAGO nvarchar(50) = 'IdSubTipoPago',
	@REFERENCIA1_ nvarchar(50) = 'Referencia1',
	@REFERENCIA2_ nvarchar(50) = 'Referencia2',
	@ID_MONEDA_CARGO nvarchar(50) = 'IdMonedaCargo',
	@CUENTA_CARGO nvarchar(50) = 'CuentaCargo',
	@MONTO_CARGO nvarchar(50) = 'MontoCargo',
	@ID_MONEDA_ABONO nvarchar(50) = 'IdMonedaAbono',
	@CUENTA_ABONO nvarchar(50) = 'CuentaAbono',
	@CUENTA_GASTO nvarchar(50) = 'CuentaGasto',
	@MONTO_ABONO nvarchar(50) = 'MontoAbono',
	@TIPO_CAMBIO nvarchar(50) = 'TipoCambio',
	@MODULO_RAIZ nvarchar(50) = 'ModuloRaiz',
	@DIGITO_CONTROL nvarchar(50) = 'DigitoControl',
	@INDICADOR_ nvarchar(50) = 'Indicador',
	@NRO_OPERACION nvarchar(50) = 'NroOperacion',
	@BENEFICIARIO_ nvarchar(50) = 'Beneficiario',
	@ID_TIPO_DOCUMENTO nvarchar(50) = 'IdTipoDocumento',
	@NRO_DOCUMENTO nvarchar(50) = 'NroDocumento',
	@CORREO_ nvarchar(50) = 'Correo',
	@NOMBRE_BANCO nvarchar(50) = 'NombreBanco',
	@RUC_BANCO nvarchar(50) = 'RucBanco',
	@NRO_FACTURA nvarchar(50) = 'NroFactura',
	@FECHA_FACTURA nvarchar(50) = 'FechaFactura',
	@FECHA_FIN_FACTURA nvarchar(50) = 'FechaFinFactura',
	@SIGNO_FACTURA nvarchar(50) = 'SignoFactura',
	@idTipoTransferencia nvarchar(5) = '',
	@idFormaPago nvarchar(5) = '',
	@idSubTipoPago nvarchar(5) = '',
	@referencia1 nvarchar(50) = '',
	@referencia2 nvarchar(50) = '',
	@idMonedaCargo nvarchar(5) = '',
	@cuentaCargo nvarchar(20) = '',
	@sMontoCargo nvarchar(20) = '',
	@montoCargo decimal(12, 2) = 0,
	@idMonedaAbono nvarchar(5) = '',
	@cuentaAbono nvarchar(20) = '',
	@cuentaGasto nvarchar(20) = '',
	@sMontoAbono nvarchar(20) = '',
	@montoAbono decimal(12, 2) = 0,
	@sTipocambio nvarchar(10) = '',
	@tipocambio decimal(6, 4) = 0,
	@sModuloRaiz nvarchar(5) = '',
	@moduloRaiz int = 0,
	@sDigitoControl nvarchar(5) = '',
	@digitoControl int = 0,
	@indicador nchar(1) = '',
	@nroOperacion nvarchar(20) = '',
	@beneficiario nvarchar(80) = '',
	@idTipoDocumento nvarchar(5) = '',
	@nroDocumento nvarchar(20) = '',
	@correo nvarchar(50) = '',
	@nombreBanco nvarchar(50) = '',
	@rucBanco nvarchar(50) = '',
	@nroFactura nvarchar(20) = '',
	@sFechaFactura nchar(8) = '',
	@fechaFactura date,
	@esFechaFactura bit = 0,
	@sFechaFinFactura nchar(8) = '',
	@fechaFinFactura date,
	@esFechaFinFactura bit = 0,
	@signoFactura nvarchar(5) = '',
	@usuarioHostToHost nvarchar(20) = 'Servicio Windows',
	@fechaCreacion date = GETDATE(),
	@transactionCount bit = 0;

IF (@@TRANCOUNT = 0)
BEGIN
	BEGIN TRANSACTION
	SET @transactionCount = 1;
END

IF (@idTipoOrden = @idTipoOrdenTransferencia)
BEGIN
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @ID_TIPO_TRANSFERENCIA;
	SELECT @idTipoTransferencia = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @idFormaPago = null;
	SELECT @idSubTipoPago = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @REFERENCIA1_;
	SELECT @referencia1 = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @REFERENCIA2_;
	SELECT @referencia2 = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @ID_MONEDA_CARGO;
	SELECT @idMonedaCargo = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @CUENTA_CARGO;
	SELECT @cuentaCargo = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @MONTO_CARGO;
	SELECT @sMontoCargo = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @sMontoCargo = SUBSTRING(@sMontoCargo, 1, LEN(@sMontoCargo) - 2) + '.' + SUBSTRING(@sMontoCargo, LEN(@sMontoCargo) - 1, 2);
	SELECT @montoCargo = CONVERT(decimal(12, 2), @sMontoCargo);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @ID_MONEDA_ABONO;
	SELECT @idMonedaAbono = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @CUENTA_ABONO;
	SELECT @cuentaAbono = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @cuentaGasto = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @MONTO_ABONO;
	SELECT @sMontoAbono = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @sMontoAbono = SUBSTRING(@sMontoAbono, 1, LEN(@sMontoAbono) - 2) + '.' + SUBSTRING(@sMontoAbono, LEN(@sMontoAbono) - 1, 2);
	SELECT @montoAbono = CONVERT(decimal(12, 2), @sMontoAbono);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @TIPO_CAMBIO;
	SELECT @sTipocambio = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @sTipocambio = CASE RTRIM(LTRIM(@sTipoCambio)) WHEN '' THEN '' ELSE SUBSTRING(@sTipocambio, 1, LEN(@sTipocambio) - 4) + '.' + SUBSTRING(@sTipocambio, LEN(@sTipocambio) - 3, 4) END;
	SELECT @tipoCambio = CASE @sTipoCambio WHEN '' THEN null ELSE CONVERT(decimal(6, 4), @sTipocambio) END;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @MODULO_RAIZ;
	SELECT @sModuloRaiz = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @moduloRaiz = CONVERT(int, @sModuloRaiz);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @DIGITO_CONTROL;
	SELECT @sDigitoControl = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @digitoControl = CONVERT(int, @sDigitoControl);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @INDICADOR_;
	SELECT @indicador = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @NRO_OPERACION;
	SELECT @nroOperacion = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @BENEFICIARIO_;
	SELECT @beneficiario = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @ID_TIPO_DOCUMENTO;
	SELECT @idTipoDocumento = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @NRO_DOCUMENTO;
	SELECT @nroDocumento = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @correo = null;
	SELECT @nombreBanco = null;
	SELECT @rucBanco = null;
	SELECT @nroFactura = null;
	SELECT @fechaFactura = null;
	SELECT @fechaFinFactura = null;
	SELECT @signoFactura = null;
END
ELSE IF (@idTipoOrden = @idTipoOrdenTransferenciaBcr)
BEGIN
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @ID_TIPO_TRANSFERENCIA;
	SELECT @idTipoTransferencia = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @idFormaPago = null;
	SELECT @idSubTipoPago = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @REFERENCIA1_;
	SELECT @referencia1 = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @REFERENCIA2_;
	SELECT @referencia2 = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @ID_MONEDA_CARGO;
	SELECT @idMonedaCargo = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @CUENTA_CARGO;
	SELECT @cuentaCargo = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @MONTO_CARGO;
	SELECT @sMontoCargo = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @sMontoCargo = SUBSTRING(@sMontoCargo, 1, LEN(@sMontoCargo) - 2) + '.' + SUBSTRING(@sMontoCargo, LEN(@sMontoCargo) - 1, 2);
	SELECT @montoCargo = CONVERT(decimal(12, 2), @sMontoCargo);
	SELECT @idMonedaAbono = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @CUENTA_ABONO;
	SELECT @cuentaAbono = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @CUENTA_GASTO;
	SELECT @cuentaGasto = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @montoAbono = null;
	SELECT @tipoCambio = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @MODULO_RAIZ;
	SELECT @sModuloRaiz = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @moduloRaiz = CONVERT(int, @sModuloRaiz);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @DIGITO_CONTROL;
	SELECT @sDigitoControl = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @digitoControl = CONVERT(int, @sDigitoControl);
	SELECT @indicador = null;
	SELECT @nroOperacion = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @BENEFICIARIO_;
	SELECT @beneficiario = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @ID_TIPO_DOCUMENTO;
	SELECT @idTipoDocumento = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @NRO_DOCUMENTO;
	SELECT @nroDocumento = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @correo = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @NOMBRE_BANCO;
	SELECT @nombreBanco = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @RUC_BANCO;
	SELECT @rucBanco = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @nroFactura = null;
	SELECT @fechaFactura = null;
	SELECT @fechaFinFactura = null;
	SELECT @signoFactura = null;
END
ELSE IF (@idTipoOrden = @idTipoOrdenProveedor or @idTipoOrden = @idTipoOrdenCamaraComercio)
BEGIN
	SELECT @idTipoTransferencia = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @ID_FORMA_PAGO;
	SELECT @idFormaPago = SUBSTRING(@cadena, @desde, @longitud);;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @ID_SUBTIPO_PAGO;
	SELECT @idSubTipoPago = SUBSTRING(@cadena, @desde, @longitud);;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @REFERENCIA1_;
	SELECT @referencia1 = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @REFERENCIA2_;
	SELECT @referencia2 = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @idMonedaCargo = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @CUENTA_CARGO;
	SELECT @cuentaCargo = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @montoCargo = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @ID_MONEDA_ABONO;
	SELECT @idMonedaAbono = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @CUENTA_ABONO;
	SELECT @cuentaAbono = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @cuentaGasto = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @MONTO_ABONO;
	SELECT @sMontoAbono = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @sMontoAbono = SUBSTRING(@sMontoAbono, 1, LEN(@sMontoAbono) - 2) + '.' + SUBSTRING(@sMontoAbono, LEN(@sMontoAbono) - 1, 2);
	SELECT @montoAbono = CONVERT(decimal(12, 2), @sMontoAbono);
	SELECT @tipoCambio = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @MODULO_RAIZ;
	SELECT @sModuloRaiz = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @moduloRaiz = CONVERT(int, @sModuloRaiz);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @DIGITO_CONTROL;
	SELECT @sDigitoControl = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @digitoControl = CONVERT(int, @sDigitoControl);
	SELECT @indicador = null;
	SELECT @nroOperacion = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @BENEFICIARIO_;
	SELECT @beneficiario = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @idTipoDocumento = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @NRO_DOCUMENTO;
	SELECT @nroDocumento = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @correo = null;
	SELECT @nombreBanco = null;
	SELECT @rucBanco = null;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @NRO_FACTURA;
	SELECT @nroFactura = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @FECHA_FACTURA;
	SELECT @sFechaFactura = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @esFechaFactura = dbo.fn_hth_verificarFecha(@sFechaFactura);
	SELECT @fechaFactura = CASE @esFechaFactura WHEN 0 THEN null ELSE CAST(@sFechaFactura as date) END;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @FECHA_FIN_FACTURA;
	SELECT @sFechaFinFactura = SUBSTRING(@cadena, @desde, @longitud);
	SELECT @esFechaFinFactura = dbo.fn_hth_verificarFecha(@sFechaFinFactura);
	SELECT @fechaFinFactura = CASE @esFechaFinFactura WHEN 0 THEN null ELSE CAST(@sFechaFinFactura as date) END;
	SELECT @desde = Desde, @longitud = Longitud FROM dbo.TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @SIGNO_FACTURA;
	SELECT @signoFactura = SUBSTRING(@cadena, @desde, @longitud);
END

SELECT @correlativoOrdenBancariaDetalle = ISNULL(MAX(IdOrdenBancariaDetalle), 0) FROM OrdenBancariaDetalle WHERE IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden and IdTipoOrden = @idTipoOrden;
SELECT @correlativoOrdenBancariaDetalle = @correlativoOrdenBancariaDetalle + 1;
SELECT @idOrdenBancariaDetalle = RIGHT(REPLICATE('0', 9) + CAST(@correlativoOrdenBancariaDetalle as nvarchar), 9);

INSERT INTO
	dbo.OrdenBancariaDetalle
	(
	IdOrdenBancariaDetalle,
	IdBanco,
	IdSociedad,
	IdSap,
	Anio,
	MomentoOrden,
	IdTipoOrden,
	IdTipoTransferencia,
	IdFormaPago,
	IdSubTipoPago,
	Referencia1,
	Referencia2,
	IdMonedaCargo,
	CuentaCargo,
	MontoCargo,
	IdMonedaAbono,
	CuentaAbono,
	CuentaGasto,
	MontoAbono,
	TipoCambio,
	ModuloRaiz,
	DigitoControl,
	Indicador,
	NroOperacion,
	Beneficiario,
	IdTipoDocumento,
	NroDocumento,
	Correo,
	NombreBanco,
	RucBanco,
	NroFactura,
	FechaFactura,
	FechaFinFactura,
	SignoFactura,
	UsuarioCreacion,
	FechaCreacion
	)
VALUES
	(
	@idOrdenBancariaDetalle,
	@idBanco,
	@idSociedad,
	LTRIM(RTRIM(@idSap)),
	@anio,
	@momentoOrden,
	@idTipoOrden,
	LTRIM(RTRIM(@idTipoTransferencia)),
	LTRIM(RTRIM(@idFormaPago)),
	LTRIM(RTRIM(@idSubTipoPago)),
	LTRIM(RTRIM(@referencia1)),
	LTRIM(RTRIM(@referencia2)),
	LTRIM(RTRIM(@idMonedaCargo)),
	LTRIM(RTRIM(@cuentaCargo)),
	@montoCargo,
	LTRIM(RTRIM(@idMonedaAbono)),
	LTRIM(RTRIM(@cuentaAbono)),
	LTRIM(RTRIM(@cuentaGasto)),
	@montoAbono,
	@tipocambio,
	@moduloRaiz,
	@digitoControl,
	@indicador,
	LTRIM(RTRIM(@nroOperacion)),
	LTRIM(RTRIM(@beneficiario)),
	LTRIM(RTRIM(@idTipoDocumento)),
	LTRIM(RTRIM(@nroDocumento)),
	LTRIM(RTRIM(@correo)),
	LTRIM(RTRIM(@nombreBanco)),
	LTRIM(RTRIM(@rucBanco)),
	LTRIM(RTRIM(@nroFactura)),
	@fechaFactura,
	@fechaFinFactura,
	LTRIM(RTRIM(@signoFactura)),
	@usuarioHostToHost,
	@fechaCreacion
	);	

IF (@transactionCount = 1)
BEGIN
	COMMIT TRANSACTION
END

SET @codigo = 10;
SET @mensaje = 'Se creó satisfactoriamente la orden bancaria y orden bancaria detalle.';

END TRY
BEGIN CATCH

IF (@transactionCount = 1)
BEGIN
	ROLLBACK TRANSACTION
END
print 'Error Código: ' + CAST(ERROR_NUMBER() as nvarchar)
print 'Error Línea: ' + CAST(ERROR_LINE() as nvarchar);
print 'Error Mensaje: ' + ERROR_MESSAGE();
SET @codigo = -15;
SET @mensaje = ERROR_MESSAGE();

END CATCH
return @codigo;
go

CREATE FUNCTION fn_hth_verificarFecha
/*************************************************************************
Nombre: fn_hth_verificarFecha
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
SELECT dbo.fn_hth_verificarFecha ('20181222');

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@fecha nchar(8)
)
RETURNS bit
AS
BEGIN

declare
	@esFecha bit = 0,
	@anio nchar(4),
	@mes nchar(2),
	@dia nchar(2),
	@year int = 0,
	@month int = 0,
	@day int = 0;
	
if (@fecha != '')
begin
	SELECT @anio = SUBSTRING(@fecha, 1, 4);
	SELECT @mes = SUBSTRING(@fecha, 5, 2);
	SELECT @dia = SUBSTRING(@fecha, 7, 2);
	SELECT @year = CAST(@anio as int);
	SELECT @month = CAST(@mes as int);
	SELECT @day = CAST(@dia as int);

	if (@month >= 1 and @month <= 12 and @day >= 1 and @day <= 31 and LEN(@anio) = 4)
	begin
		SET @esFecha = 1;
	end
end

return @esFecha;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasLiberadas
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasLiberadas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasLiberadas '000002', 1, 20, 0;

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@pagina int,
@filas int,
@idSociedad nchar(4),
@idBanco nchar(5),
@idTipoOrden nvarchar(5),
@totalRegistros int OUTPUT
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idEstadoOrdenLiberado nchar(2) = 'LI',
	@desde int = 0,
	@hasta int = 0;

SELECT @desde = CASE @pagina WHEN 1 THEN 1 ELSE ((@pagina - 1) * @filas) + 1 END;
SELECT @hasta = @desde + @filas - 1;

SELECT
	@totalRegistros = COUNT(IdOrdenBancaria)
FROM
	OrdenBancaria
WHERE
	(@idSociedad = '' or IdSociedad = @idSociedad) and (@idBanco = '' or IdBanco = @idBanco)
	and (@idTipoOrden = '' or IdTipoOrden = @idTipoOrden)
	and IdEstadoOrden in (@idEstadoOrdenLiberado)
	and IdUsuario = @idUsuario;

WITH ordenBancaria (Id, IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden,
EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo,
MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador,
UsuarioCreacion, FechaCreacion)
AS
(
	SELECT
		ROW_NUMBER() OVER (ORDER BY ob.FechaCreacion DESC, ob.FechaOrden DESC),
		ob.IdOrdenBancaria,
		b.Banco as Banco,
		b.Abreviacion as BancoCorto,
		ob.IdTipoOrden as IdTipoOrden,
		too.TipoOrden,
		too.Abreviacion as TipoOrdenCorto,
		ob.IdEstadoOrden as IdEstadoOrden,
		eo.EstadoOrden as EstadoOrden,
		ob.IdSociedad as IdSociedad,
		s.Sociedad as Sociedad,
		s.Abreviacion as SociedadCorto,
		ob.IdSap as IdSap,
		ob.Anio as Anio,
		ob.MomentoOrden as MomentoOrden,
		CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
		ob.NombreArchivo as NombreArchivo,
		ob.RutaArchivo,
		dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
		dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
		dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
		dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
		dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
		dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
		dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
		au.UserName as UsuarioCreacion,
		ob.FechaCreacion as FechaCreacion
	FROM
		dbo.OrdenBancaria ob
		join dbo.AspNetUsers au on ob.IdUsuario = au.IdUsuario
		join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
		join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
		join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
		join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
	WHERE
		ob.IdUsuario = @idUsuario and ob.IdEstadoOrden = @idEstadoOrdenLiberado
		and (@idSociedad = '' or ob.IdSociedad = @idSociedad) and (@idBanco = '' or ob.IdBanco = @idBanco)
		and (@idTipoOrden = '' or ob.IdTipoOrden = @idTipoOrden)
)

SELECT
	IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden,
	EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo,
	MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador,
	UsuarioCreacion, FechaCreacion
FROM
	ordenBancaria
WHERE
	CAST(Id as int) >= @desde and CAST(Id as int) <= @hasta;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasDetalleLiberadas
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasDetalleLiberadas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasDetalleLiberadas '0010', '2200000054', '2018', '20180101';
exec spw_hth_listarOrdenesBancariasDetalleLiberadas '0010', '00001R', '2018', '20181129';
exec spw_hth_listarOrdenesBancariasDetalleLiberadas '0010', 'GL001', '2016', '20160401';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idEstadoOrdenLiberado nchar(2) = 'LI';

SELECT
	obd.IdOrdenBancariaDetalle,
	tt.TipoTransferencia as TipoTransferencia,
	fp.FormaPago as FormaPago,
	stp.SubTipoPago,
	Referencia1 as Referencia1,
	Referencia2 as Referencia2,
	tm1.TipoMoneda as MonedaCargo,
	tm1.Simbolo as MonedaCargoCorto,
	obd.CuentaCargo as CuentaCargo,
	obd.MontoCargo as MontoCargo,
	tm2.TipoMoneda as MonedaAbono,
	tm2.Simbolo as MonedaAbonoCorto,
	obd.CuentaAbono as CuentaAbono,
	obd.CuentaGasto as CuentaGasto,
	obd.MontoAbono as MontoAbono,
	obd.TipoCambio as TipoCambio,
	obd.ModuloRaiz as ModuloRaiz,
	obd.DigitoControl as DigitoControl,
	CASE obd.Indicador WHEN '0' THEN 'MISMA MONEDA' WHEN '1' THEN 'RECALCULO ABONO' WHEN '2' THEN 'RECALCULO CARGO' ELSE '' END as Indicador,
	obd.NroOperacion,
	obd.Beneficiario,
	td.TipoDocumento as TipoDocumento,
	td.Abreviacion as TipoDocumentoCorto,
	obd.NroDocumento,
	obd.Correo,
	obd.NombreBanco,
	obd.RucBanco,
	obd.NroFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFactura, null), 103) as FechaFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFinFactura, null), 103) as FechaFinFactura,
	CASE obd.SignoFactura WHEN '+' THEN 'FACTURA' WHEN '-' THEN 'NOTA CRÉDITO' ELSE '' END as SignoFactura,
	obd.UsuarioCreacion,
	obd.FechaCreacion
FROM
	OrdenBancaria ob
	join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	left join TipoTransferencia tt on obd.IdBanco = tt.IdBanco and obd.IdTipoTransferencia = tt.IdTipoTransferencia and tt.Estado = 1
	left join FormaPago fp on obd.IdBanco = fp.IdBanco and obd.IdFormaPago = fp.IdFormaPago and fp.Estado = 1
	left join SubTipoPago stp on obd.IdBanco = stp.IdBanco and obd.IdSubTipoPago = stp.IdSubTipoPago and stp.Estado = 1
	left join TipoMoneda tm1 on obd.IdBanco = tm1.IdBanco and obd.IdMonedaCargo = tm1.IdTipoMoneda and tm1.Estado = 1
	left join TipoMoneda tm2 on obd.IdBanco = tm2.IdBanco and obd.IdMonedaAbono = tm2.IdTipoMoneda and tm2.Estado = 1
	left join TipoDocumento td on obd.IdBanco = td.IdBanco and obd.IdTipoDocumento = td.IdTipoDocumento and td.Estado = 1
WHERE
	ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
	and ob.IdTipoOrden = @idTipoOrden and ob.IdEstadoOrden in (@idEstadoOrdenLiberado)
ORDER BY
	obd.FechaCreacion ASC;

END
go

CREATE PROCEDURE spw_hth_buscarOrdenesBancariasLiberadas
/*************************************************************************
Nombre: spw_hth_buscarOrdenesBancariasLiberadas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_buscarOrdenesBancariasLiberadas '000002', '2018/01/01', '2018/01/31';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@idSap nvarchar(10),
@fechaInicio nchar(10),
@fechaFin nchar(10)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idEstadoOrdenLiberado nchar(2) = 'LI';

SELECT
	ob.IdOrdenBancaria,
	b.Banco as Banco,
	b.Abreviacion as BancoCorto,
	ob.IdTipoOrden as IdTipoOrden,
	too.TipoOrden,
	too.Abreviacion as TipoOrdenCorto,
	ob.IdEstadoOrden as IdEstadoOrden,
	eo.EstadoOrden as EstadoOrden,
	ob.IdSociedad as IdSociedad,
	s.Sociedad as Sociedad,
	s.Abreviacion as SociedadCorto,
	ob.IdSap as IdSap,
	ob.Anio as Anio,
	ob.MomentoOrden as MomentoOrden,
	CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
	ob.NombreArchivo as NombreArchivo,
	ob.RutaArchivo,
	dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
	dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
	dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
	dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
	dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
	dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
	dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
	au.UserName as UsuarioCreacion,
	ob.FechaCreacion as FechaCreacion
FROM
	dbo.OrdenBancaria ob
	join dbo.AspNetUsers au on ob.IdUsuario = au.IdUsuario
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
	join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
	join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	ob.IdUsuario = @idUsuario and ob.IdEstadoOrden = @idEstadoOrdenLiberado
	and (@idSap = '' or ob.IdSap like '%' + @idSap + '%')
	and (@fechaInicio = '' and @fechaFin = '' or ob.FechaOrden >= CAST(@fechaInicio as date) and ob.FechaOrden <= CAST(@fechaFin as date))
ORDER BY
	ob.FechaCreacion DESC;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasDeshechas
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasDeshechas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasDeshechas '000002';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@pagina int,
@filas int,
@idSociedad nchar(4),
@idBanco nchar(5),
@idTipoOrden nvarchar(5),
@totalRegistros int OUTPUT
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idEstadoOrdenDeshecho nchar(2) = 'DE',
	@idEstadoOrdenAnulado nchar(2) = 'AN',
	@desde int = 0,
	@hasta int = 0;

SELECT @desde = CASE @pagina WHEN 1 THEN 1 ELSE ((@pagina - 1) * @filas) + 1 END;
SELECT @hasta = @desde + @filas - 1;

SELECT
	@totalRegistros = COUNT(IdOrdenBancaria)
FROM
	OrdenBancaria
WHERE
	(@idSociedad = '' or IdSociedad = @idSociedad) and (@idBanco = '' or IdBanco = @idBanco) 
	and (@idTipoOrden = '' or IdTipoOrden = @idTipoOrden)
	and IdEstadoOrden in (@idEstadoOrdenDeshecho, @idEstadoOrdenAnulado)
	and IdUsuario = @idUsuario;

WITH ordenBancaria (Id, IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden,
EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo,
MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador,
PreAprobador, Anulador, Comentarios, UsuarioCreacion, FechaCreacion, UsuarioEdicion, FechaEdicion)
AS
(
	SELECT
		ROW_NUMBER() OVER (ORDER BY ob.FechaCreacion DESC, ob.FechaOrden DESC),
		ob.IdOrdenBancaria,
		b.Banco as Banco,
		b.Abreviacion as BancoCorto,
		ob.IdTipoOrden as IdTipoOrden,
		too.TipoOrden,
		too.Abreviacion as TipoOrdenCorto,
		ob.IdEstadoOrden as IdEstadoOrden,
		eo.EstadoOrden as EstadoOrden,
		ob.IdSociedad as IdSociedad,
		s.Sociedad as Sociedad,
		s.Abreviacion as SociedadCorto,
		ob.IdSap as IdSap,
		ob.Anio as Anio,
		ob.MomentoOrden as MomentoOrden,
		CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
		ob.NombreArchivo as NombreArchivo,
		ob.RutaArchivo,
		dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
		dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
		dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
		dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
		dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
		dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
		dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
		dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as PreAprobador,
		dbo.fn_hth_obtenerAnulador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Aprobador,
		dbo.fn_hth_obtenerComentarios(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdEstadoOrden),
		au.UserName as UsuarioCreacion,
		ob.FechaCreacion as FechaCreacion,
		ob.UsuarioEdicion,
		ob.FechaEdicion
	FROM
		dbo.OrdenBancaria ob
		join dbo.AspNetUsers au on ob.IdUsuario = au.IdUsuario
		join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
		join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
		join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
		join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
	WHERE
		ob.IdUsuario = @idUsuario and ob.IdEstadoOrden in (@idEstadoOrdenDeshecho, @idEstadoOrdenAnulado)
		and (@idSociedad = '' or ob.IdSociedad = @idSociedad) and (@idBanco = '' or ob.IdBanco = @idBanco) 
		and (@idTipoOrden = '' or ob.IdTipoOrden = @idTipoOrden)
)

SELECT
	IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden,
	EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo,
	MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador,
	PreAprobador, Anulador, Comentarios, UsuarioCreacion, FechaCreacion, UsuarioEdicion, FechaEdicion
FROM
	ordenBancaria
WHERE
	CAST(Id as int) >= @desde and CAST(Id as int) <= @hasta;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasDetalleDeshechas
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasDetalleDeshechas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasDetalleDeshechas '0010', '2200000054', '2018', '20180101';
exec spw_hth_listarOrdenesBancariasDetalleDeshechas '0010', '00001R', '2018', '20181129';
exec spw_hth_listarOrdenesBancariasDetalleDeshechas '0010', 'GL001', '2016', '20160401';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idEstadoOrdenDeshecho nchar(2) = 'DE',
	@idEstadoOrdenAnulado nchar(2) = 'AN';

SELECT
	obd.IdOrdenBancariaDetalle,
	tt.TipoTransferencia as TipoTransferencia,
	fp.FormaPago as FormaPago,
	stp.SubTipoPago,
	Referencia1 as Referencia1,
	Referencia2 as Referencia2,
	tm1.TipoMoneda as MonedaCargo,
	tm1.Simbolo as MonedaCargoCorto,
	obd.CuentaCargo as CuentaCargo,
	obd.MontoCargo as MontoCargo,
	tm2.TipoMoneda as MonedaAbono,
	tm2.Simbolo as MonedaAbonoCorto,
	obd.CuentaAbono as CuentaAbono,
	obd.CuentaGasto as CuentaGasto,
	obd.MontoAbono as MontoAbono,
	obd.TipoCambio as TipoCambio,
	obd.ModuloRaiz as ModuloRaiz,
	obd.DigitoControl as DigitoControl,
	CASE obd.Indicador WHEN '0' THEN 'MISMA MONEDA' WHEN '1' THEN 'RECALCULO ABONO' WHEN '2' THEN 'RECALCULO CARGO' ELSE '' END as Indicador,
	obd.NroOperacion,
	obd.Beneficiario,
	td.TipoDocumento as TipoDocumento,
	td.Abreviacion as TipoDocumentoCorto,
	obd.NroDocumento,
	obd.Correo,
	obd.NombreBanco,
	obd.RucBanco,
	obd.NroFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFactura, null), 103) as FechaFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFinFactura, null), 103) as FechaFinFactura,
	CASE obd.SignoFactura WHEN '+' THEN 'FACTURA' WHEN '-' THEN 'NOTA CRÉDITO' ELSE '' END as SignoFactura,
	obd.UsuarioCreacion,
	obd.FechaCreacion
FROM
	OrdenBancaria ob
	join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	left join TipoTransferencia tt on obd.IdBanco = tt.IdBanco and obd.IdTipoTransferencia = tt.IdTipoTransferencia and tt.Estado = 1
	left join FormaPago fp on obd.IdBanco = fp.IdBanco and obd.IdFormaPago = fp.IdFormaPago and fp.Estado = 1
	left join SubTipoPago stp on obd.IdBanco = stp.IdBanco and obd.IdSubTipoPago = stp.IdSubTipoPago and stp.Estado = 1
	left join TipoMoneda tm1 on obd.IdBanco = tm1.IdBanco and obd.IdMonedaCargo = tm1.IdTipoMoneda and tm1.Estado = 1
	left join TipoMoneda tm2 on obd.IdBanco = tm2.IdBanco and obd.IdMonedaAbono = tm2.IdTipoMoneda and tm2.Estado = 1
	left join TipoDocumento td on obd.IdBanco = td.IdBanco and obd.IdTipoDocumento = td.IdTipoDocumento and td.Estado = 1
WHERE
	ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
	and ob.IdTipoOrden = @idTipoOrden and ob.IdEstadoOrden in (@idEstadoOrdenDeshecho, @idEstadoOrdenAnulado)
ORDER BY
	obd.FechaCreacion ASC;

END
go

CREATE PROCEDURE spw_hth_buscarOrdenesBancariasDeshechas
/*************************************************************************
Nombre: spw_hth_buscarOrdenesBancariasDeshechas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_buscarOrdenesBancariasDeshechas '000002', 'LI', '2018/01/01', '2018/01/31';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@idSap nvarchar(10),
@fechaInicio nchar(10),
@fechaFin nchar(10)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idEstadoOrdenDeshecho nchar(2) = 'DE',
	@idEstadoOrdenAnulado nchar(2) = 'AN';

SELECT
	ob.IdOrdenBancaria,
	b.Banco as Banco,
	b.Abreviacion as BancoCorto,
	ob.IdTipoOrden as IdTipoOrden,
	too.TipoOrden,
	too.Abreviacion as TipoOrdenCorto,
	ob.IdEstadoOrden as IdEstadoOrden,
	eo.EstadoOrden as EstadoOrden,
	ob.IdSociedad as IdSociedad,
	s.Sociedad as Sociedad,
	s.Abreviacion as SociedadCorto,
	ob.IdSap as IdSap,
	ob.Anio as Anio,
	ob.MomentoOrden as MomentoOrden,
	CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
	ob.NombreArchivo as NombreArchivo,
	ob.RutaArchivo,
	dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
	dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
	dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
	dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
	dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
	dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
	dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
	dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as PreAprobador,
	dbo.fn_hth_obtenerAnulador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Aprobador,
	dbo.fn_hth_obtenerComentarios(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdEstadoOrden),
	au.UserName as UsuarioCreacion,
	ob.FechaCreacion as FechaCreacion,
	ob.UsuarioEdicion,
	ob.FechaEdicion
FROM
	dbo.OrdenBancaria ob
	join dbo.AspNetUsers au on ob.IdUsuario = au.IdUsuario
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
	join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
	join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	ob.IdUsuario = @idUsuario and ob.IdEstadoOrden in (@idEstadoOrdenDeshecho, @idEstadoOrdenAnulado)
	and (@idSap = '' or ob.IdSap like '%' + @idSap + '%')
	and (@fechaInicio = '' and @fechaFin = '' or ob.FechaOrden >= CAST(@fechaInicio as date) and ob.FechaOrden <= CAST(@fechaFin as date))
ORDER BY
	ob.FechaCreacion DESC;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasPorAprobar
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasPorAprobar
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasPorAprobar '000002', 1, 20, '', '', '', 0;

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@pagina int,
@filas int,
@idSociedad nchar(4),
@idBanco nchar(5),
@idTipoOrden nvarchar(5),
@idEstadoOrden nchar(2),
@totalRegistros int OUTPUT
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@usuario nvarchar(20) = '',
	@idEstadoOrdenLiberado nchar(2) = 'LI',
	@idEstadoOrdenPreAprobado nchar(2) = 'PP',
	@desde int = 0,
	@hasta int = 0;

SELECT @usuario = UserName FROM dbo.AspNetUsers WHERE IdUsuario = @IdUsuario;
SELECT @desde = CASE @pagina WHEN 1 THEN 1 ELSE ((@pagina - 1) * @filas) + 1 END;
SELECT @hasta = @desde + @filas - 1;

SELECT
	@totalRegistros = COUNT(IdOrdenBancaria)
FROM
	OrdenBancaria
WHERE
	(@idSociedad = '' or IdSociedad = @idSociedad) and (@idBanco = '' or IdBanco = @idBanco) 
	and (@idTipoOrden = '' or IdTipoOrden = @idTipoOrden) and (@idEstadoOrden = '' or IdEstadoOrden = @idEstadoOrden)
	and IdEstadoOrden in (@idEstadoOrdenLiberado, @idEstadoOrdenPreAprobado)
	and dbo.fn_hth_obtenerPreAprobador(IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden) <> @usuario;

WITH ordenBancaria (Id, IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden,
EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo, 
MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador, PreAprobador,
UsuarioCreacion, FechaCreacion)
AS
(
	SELECT
		ROW_NUMBER() OVER (ORDER BY ob.FechaCreacion DESC, ob.FechaOrden DESC),
		ob.IdOrdenBancaria,
		b.Banco as Banco,
		b.Abreviacion as BancoCorto,
		ob.IdTipoOrden as IdTipoOrden,
		too.TipoOrden,
		too.Abreviacion as TipoOrdenCorto,
		ob.IdEstadoOrden as IdEstadoOrden,
		eo.EstadoOrden as EstadoOrden,
		ob.IdSociedad as IdSociedad,
		s.Sociedad as Sociedad,
		s.Abreviacion as SociedadCorto,
		ob.IdSap as IdSap,
		ob.Anio as Anio,
		ob.MomentoOrden as MomentoOrden,
		CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
		ob.NombreArchivo as NombreArchivo,
		ob.RutaArchivo,
		dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
		dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
		dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
		dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
		dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
		dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
		dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
		dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as PreAprobador,
		au.UserName as UsuarioCreacion,
		ob.FechaCreacion as FechaCreacion
	FROM
		dbo.OrdenBancaria ob
		join Usuario u on ob.IdUsuario = u.IdUsuario and u.Estado = 1
		join dbo.AspNetUsers au on u.IdUsuario = au.IdUsuario
		join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
		join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
		join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
		join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
	WHERE
		ob.IdEstadoOrden in (@idEstadoOrdenLiberado, @idEstadoOrdenPreAprobado)
		and (@idSociedad = '' or ob.IdSociedad = @idSociedad) and (@idBanco = '' or ob.IdBanco = @idBanco) 
		and (@idTipoOrden = '' or ob.IdTipoOrden = @idTipoOrden) and (@idEstadoOrden = '' or ob.IdEstadoOrden = @idEstadoOrden)
	GROUP BY
		ob.IdOrdenBancaria,	ob.IdBanco, b.Banco, b.Abreviacion, ob.IdTipoOrden, too.TipoOrden, too.Abreviacion,
		ob.IdEstadoOrden, eo.EstadoOrden, ob.IdSociedad, s.Sociedad, s.Abreviacion, ob.IdSap, ob.Anio, ob.MomentoOrden,
		ob.FechaOrden, ob.NombreArchivo, ob.RutaArchivo, au.UserName, ob.FechaCreacion
	HAVING
		dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) <> @usuario
)

SELECT
	IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden,
	EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo, 
	MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador, PreAprobador,
	UsuarioCreacion, FechaCreacion
FROM
	ordenBancaria
WHERE
	CAST(Id as int) >= @desde and CAST(Id as int) <= @hasta;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasDetallePorAprobar
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasDetallePorAprobar
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasDetallePorAprobar '0010', '2200000054', '2018', '20180101';
exec spw_hth_listarOrdenesBancariasDetallePorAprobar '0010', '00001R', '2018', '20181129';
exec spw_hth_listarOrdenesBancariasDetallePorAprobar '0010', 'GL001', '2016', '20160401';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idEstadoOrdenLiberado nchar(2) = 'LI',
	@idEstadoOrdenPreAprobado nchar(2) = 'PP';

SELECT
	obd.IdOrdenBancariaDetalle,
	tt.TipoTransferencia as TipoTransferencia,
	fp.FormaPago as FormaPago,
	stp.SubTipoPago,
	Referencia1 as Referencia1,
	Referencia2 as Referencia2,
	tm1.TipoMoneda as MonedaCargo,
	tm1.Simbolo as MonedaCargoCorto,
	obd.CuentaCargo as CuentaCargo,
	obd.MontoCargo as MontoCargo,
	tm2.TipoMoneda as MonedaAbono,
	tm2.Simbolo as MonedaAbonoCorto,
	obd.CuentaAbono as CuentaAbono,
	obd.CuentaGasto as CuentaGasto,
	obd.MontoAbono as MontoAbono,
	obd.TipoCambio as TipoCambio,
	obd.ModuloRaiz as ModuloRaiz,
	obd.DigitoControl as DigitoControl,
	CASE obd.Indicador WHEN '0' THEN 'MISMA MONEDA' WHEN '1' THEN 'RECALCULO ABONO' WHEN '2' THEN 'RECALCULO CARGO' ELSE '' END as Indicador,
	obd.NroOperacion,
	obd.Beneficiario,
	td.TipoDocumento as TipoDocumento,
	td.Abreviacion as TipoDocumentoCorto,
	obd.NroDocumento,
	obd.Correo,
	obd.NombreBanco,
	obd.RucBanco,
	obd.NroFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFactura, null), 103) as FechaFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFinFactura, null), 103) as FechaFinFactura,
	CASE obd.SignoFactura WHEN '+' THEN 'FACTURA' WHEN '-' THEN 'NOTA CRÉDITO' ELSE '' END as SignoFactura,
	obd.UsuarioCreacion,
	obd.FechaCreacion
FROM
	OrdenBancaria ob
	join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	left join TipoTransferencia tt on obd.IdBanco = tt.IdBanco and obd.IdTipoTransferencia = tt.IdTipoTransferencia and tt.Estado = 1
	left join FormaPago fp on obd.IdBanco = fp.IdBanco and obd.IdFormaPago = fp.IdFormaPago and fp.Estado = 1
	left join SubTipoPago stp on obd.IdBanco = stp.IdBanco and obd.IdSubTipoPago = stp.IdSubTipoPago and stp.Estado = 1
	left join TipoMoneda tm1 on obd.IdBanco = tm1.IdBanco and obd.IdMonedaCargo = tm1.IdTipoMoneda and tm1.Estado = 1
	left join TipoMoneda tm2 on obd.IdBanco = tm2.IdBanco and obd.IdMonedaAbono = tm2.IdTipoMoneda and tm2.Estado = 1
	left join TipoDocumento td on obd.IdBanco = td.IdBanco and obd.IdTipoDocumento = td.IdTipoDocumento and td.Estado = 1
WHERE
	ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
	and ob.IdTipoOrden = @idTipoOrden and ob.IdEstadoOrden in (@idEstadoOrdenLiberado, @idEstadoOrdenPreAprobado)
ORDER BY
	obd.FechaCreacion ASC;

END
go

CREATE PROCEDURE spw_hth_buscarOrdenesBancariasPorAprobar
/*************************************************************************
Nombre: spw_hth_buscarOrdenesBancariasPorAprobar
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_buscarOrdenesBancariasPorAprobar '000003', '2018/01/01', '2018/01/31';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@idSap nvarchar(10),
@fechaInicio nchar(10),
@fechaFin nchar(10)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@usuario nvarchar(20) = '',
	@idEstadoOrdenLiberado nchar(2) = 'LI',
	@idEstadoOrdenPreAprobado nchar(2) = 'PP';

SELECT @usuario = UserName FROM dbo.AspNetUsers WHERE IdUsuario = @IdUsuario;

SELECT
	ob.IdOrdenBancaria,
	b.Banco as Banco,
	b.Abreviacion as BancoCorto,
	ob.IdTipoOrden as IdTipoOrden,
	too.TipoOrden,
	too.Abreviacion as TipoOrdenCorto,
	ob.IdEstadoOrden as IdEstadoOrden,
	eo.EstadoOrden as EstadoOrden,
	ob.IdSociedad as IdSociedad,
	s.Sociedad as Sociedad,
	s.Abreviacion as SociedadCorto,
	ob.IdSap as IdSap,
	ob.Anio as Anio,
	ob.MomentoOrden as MomentoOrden,
	CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
	ob.NombreArchivo as NombreArchivo,
	ob.RutaArchivo,
	dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
	dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
	dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
	dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
	dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
	dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
	dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
	dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as PreAprobador,
	au.UserName as UsuarioCreacion,
	ob.FechaCreacion as FechaCreacion
FROM
	dbo.OrdenBancaria ob
	join Usuario u on ob.IdUsuario = u.IdUsuario and u.Estado = 1
	join dbo.AspNetUsers au on u.IdUsuario = au.IdUsuario
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
	join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
	join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenLiberado, @idEstadoOrdenPreAprobado)
	and (@idSap = '' or ob.IdSap like '%' + @idSap + '%')
	and (@fechaInicio = '' and @fechaFin = '' or ob.FechaOrden >= CAST(@fechaInicio as date) and ob.FechaOrden <= CAST(@fechaFin as date))
GROUP BY
	ob.IdOrdenBancaria,	ob.IdBanco, b.Banco, b.Abreviacion, ob.IdTipoOrden, too.TipoOrden, too.Abreviacion,
	ob.IdEstadoOrden, eo.EstadoOrden, ob.IdSociedad, s.Sociedad, s.Abreviacion, ob.IdSap, ob.Anio, ob.MomentoOrden,
	ob.FechaOrden, ob.NombreArchivo, ob.RutaArchivo, au.UserName, ob.FechaCreacion
HAVING
	dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) <> @usuario
ORDER BY
	ob.FechaCreacion DESC;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasAprobadas
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasAprobadas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasAprobadas '000001', 1, 20, '', '', '', 0;

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@pagina int,
@filas int,
@idSociedad nchar(4),
@idBanco nchar(5),
@idTipoOrden nvarchar(5),
@totalRegistros int OUTPUT
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@usuario nvarchar(20) = '',
	@idEstadoOrdenPreAprobado nchar(2) = 'PP',
	@idEstadoOrdenAprobado nchar(2) = 'AP',
	@idEstadoOrdenEntregado nchar(2) = 'ET',
	@idEstadoOrdenPagado nchar(2) = 'PA',
	@idEstadoOrdenConErrores nchar(2) = 'CE',
	@desde int = 0,
	@hasta int = 0;

SELECT @usuario = UserName FROM dbo.AspNetUsers WHERE IdUsuario = @IdUsuario;
SELECT @desde = CASE @pagina WHEN 1 THEN 1 ELSE ((@pagina - 1) * @filas) + 1 END;
SELECT @hasta = @desde + @filas - 1;

SELECT
	@totalRegistros = COUNT(IdOrdenBancaria)
FROM
	OrdenBancaria
WHERE
	(@idSociedad = '' or IdSociedad = @idSociedad) and (@idBanco = '' or IdBanco = @idBanco) 
	and (@idTipoOrden = '' or IdTipoOrden = @idTipoOrden)
	and IdEstadoOrden in (@idEstadoOrdenPreAprobado, @idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
	and (dbo.fn_hth_obtenerPreAprobador(IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden) = @usuario or dbo.fn_hth_obtenerAprobador(IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden) = @usuario);

WITH ordenBancaria (Id, IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden, 
EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo, 
MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador, PreAprobador, 
Aprobador, Comentarios, UsuarioCreacion, FechaCreacion, UsuarioEdicion, FechaEdicion)
AS
(
	SELECT
		ROW_NUMBER() OVER (ORDER BY ob.FechaCreacion DESC, ob.FechaOrden DESC),
		ob.IdOrdenBancaria,
		b.Banco as Banco,
		b.Abreviacion as BancoCorto,
		ob.IdTipoOrden as IdTipoOrden,
		too.TipoOrden,
		too.Abreviacion as TipoOrdenCorto,
		ob.IdEstadoOrden as IdEstadoOrden,
		eo.EstadoOrden as EstadoOrden,
		ob.IdSociedad as IdSociedad,
		s.Sociedad as Sociedad,
		s.Abreviacion as SociedadCorto,
		ob.IdSap as IdSap,
		ob.Anio as Anio,
		ob.MomentoOrden as MomentoOrden,
		CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
		ob.NombreArchivo as NombreArchivo,
		ob.RutaArchivo,
		dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
		dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
		dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
		dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
		dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
		dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
		dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
		dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as PreAprobador,
		dbo.fn_hth_obtenerAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Aprobador,
		dbo.fn_hth_obtenerComentarios(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdEstadoOrden),
		au.UserName as UsuarioCreacion,
		ob.FechaCreacion as FechaCreacion,
		ob.UsuarioEdicion,
		ob.FechaEdicion
	FROM
		dbo.OrdenBancaria ob
		join Usuario u on ob.IdUsuario = u.IdUsuario and u.Estado = 1
		join dbo.AspNetUsers au on u.IdUsuario = au.IdUsuario
		join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
		join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
		join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
		join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
	WHERE
		ob.IdEstadoOrden in (@idEstadoOrdenPreAprobado, @idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
		and (@idSociedad = '' or ob.IdSociedad = @idSociedad) and (@idBanco = '' or ob.IdBanco = @idBanco) 
		and (@idTipoOrden = '' or ob.IdTipoOrden = @idTipoOrden)
	GROUP BY
		ob.IdOrdenBancaria,	ob.IdBanco, b.Banco, b.Abreviacion, ob.IdTipoOrden, too.TipoOrden, too.Abreviacion,
		ob.IdEstadoOrden, eo.EstadoOrden, ob.IdSociedad, s.Sociedad, s.Abreviacion, ob.IdSap, ob.Anio, ob.MomentoOrden,
		ob.FechaOrden, ob.NombreArchivo, ob.RutaArchivo, au.UserName, ob.FechaCreacion, ob.UsuarioEdicion, ob.FechaEdicion
	HAVING
		(dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario 
		or dbo.fn_hth_obtenerAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario)		
)

SELECT
	IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden, 
	EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo, 
	MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador, PreAprobador, 
	Aprobador, Comentarios, UsuarioCreacion, FechaCreacion, UsuarioEdicion, FechaEdicion
FROM
	ordenBancaria
WHERE
	CAST(Id as int) >= @desde and CAST(Id as int) <= @hasta;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasDetalleAprobadas
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasDetalleAprobadas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasDetalleAprobadas '0010', '2200000054', '2018', '20180101';
exec spw_hth_listarOrdenesBancariasDetalleAprobadas '0010', '00001R', '2018', '20181129';
exec spw_hth_listarOrdenesBancariasDetalleAprobadas '0010', 'GL001', '2016', '20160401';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5)
)
AS
SET NOCOUNT ON;
BEGIN

declare
	@idEstadoOrdenPreAprobado nchar(2) = 'PP',
	@idEstadoOrdenAprobado nchar(2) = 'AP',
	@idEstadoOrdenEntregado nchar(2) = 'ET',
	@idEstadoOrdenPagado nchar(2) = 'PA',
	@idEstadoOrdenConErrores nchar(2) = 'CE';

SELECT
	obd.IdOrdenBancariaDetalle,
	tt.TipoTransferencia as TipoTransferencia,
	fp.FormaPago as FormaPago,
	stp.SubTipoPago,
	Referencia1 as Referencia1,
	Referencia2 as Referencia2,
	tm1.TipoMoneda as MonedaCargo,
	tm1.Simbolo as MonedaCargoCorto,
	obd.CuentaCargo as CuentaCargo,
	obd.MontoCargo as MontoCargo,
	tm2.TipoMoneda as MonedaAbono,
	tm2.Simbolo as MonedaAbonoCorto,
	obd.CuentaAbono as CuentaAbono,
	obd.CuentaGasto as CuentaGasto,
	obd.MontoAbono as MontoAbono,
	obd.TipoCambio as TipoCambio,
	obd.ModuloRaiz as ModuloRaiz,
	obd.DigitoControl as DigitoControl,
	CASE obd.Indicador WHEN '0' THEN 'MISMA MONEDA' WHEN '1' THEN 'RECALCULO ABONO' WHEN '2' THEN 'RECALCULO CARGO' ELSE '' END as Indicador,
	obd.NroOperacion,
	obd.Beneficiario,
	td.TipoDocumento as TipoDocumento,
	td.Abreviacion as TipoDocumentoCorto,
	obd.NroDocumento,
	obd.Correo,
	obd.NombreBanco,
	obd.RucBanco,
	obd.NroFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFactura, null), 103) as FechaFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFinFactura, null), 103) as FechaFinFactura,
	CASE obd.SignoFactura WHEN '+' THEN 'FACTURA' WHEN '-' THEN 'NOTA CRÉDITO' ELSE '' END as SignoFactura,
	obd.IdRespuesta,
	obd.Respuesta,
	obd.NroOrden,
	obd.NroConvenio,
	obd.UsuarioCreacion,
	obd.FechaCreacion
FROM
	OrdenBancaria ob
	join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	left join TipoTransferencia tt on obd.IdBanco = tt.IdBanco and obd.IdTipoTransferencia = tt.IdTipoTransferencia and tt.Estado = 1
	left join FormaPago fp on obd.IdBanco = fp.IdBanco and obd.IdFormaPago = fp.IdFormaPago and fp.Estado = 1
	left join SubTipoPago stp on obd.IdBanco = stp.IdBanco and obd.IdSubTipoPago = stp.IdSubTipoPago and stp.Estado = 1
	left join TipoMoneda tm1 on obd.IdBanco = tm1.IdBanco and obd.IdMonedaCargo = tm1.IdTipoMoneda and tm1.Estado = 1
	left join TipoMoneda tm2 on obd.IdBanco = tm2.IdBanco and obd.IdMonedaAbono = tm2.IdTipoMoneda and tm2.Estado = 1
	left join TipoDocumento td on obd.IdBanco = td.IdBanco and obd.IdTipoDocumento = td.IdTipoDocumento and td.Estado = 1
WHERE
	ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
	and ob.IdTipoOrden = @idTipoOrden and ob.IdEstadoOrden 
	in (@idEstadoOrdenPreAprobado, @idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
ORDER BY
	ob.FechaCreacion ASC;

END
go

CREATE PROCEDURE spw_hth_buscarOrdenesBancariasAprobadas
/*************************************************************************
Nombre: spw_hth_buscarOrdenesBancariasAprobadas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_buscarOrdenesBancariasAprobadas '2018/01/01', '2018/01/31';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@idSap nvarchar(10),
@fechaInicio nchar(10),
@fechaFin nchar(10)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@usuario nvarchar(20) = '',
	@idEstadoOrdenPreAprobado nchar(2) = 'PP',
	@idEstadoOrdenAprobado nchar(2) = 'AP',
	@idEstadoOrdenEntregado nchar(2) = 'ET',
	@idEstadoOrdenPagado nchar(2) = 'PA',
	@idEstadoOrdenConErrores nchar(2) = 'CE';

SELECT @usuario = UserName FROM dbo.AspNetUsers WHERE IdUsuario = @IdUsuario;

SELECT
	ob.IdOrdenBancaria,
	b.Banco as Banco,
	b.Abreviacion as BancoCorto,
	ob.IdTipoOrden as IdTipoOrden,
	too.TipoOrden,
	too.Abreviacion as TipoOrdenCorto,
	ob.IdEstadoOrden as IdEstadoOrden,
	eo.EstadoOrden as EstadoOrden,
	ob.IdSociedad as IdSociedad,
	s.Sociedad as Sociedad,
	s.Abreviacion as SociedadCorto,
	ob.IdSap as IdSap,
	ob.Anio as Anio,
	ob.MomentoOrden as MomentoOrden,
	CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
	ob.NombreArchivo as NombreArchivo,
	ob.RutaArchivo,
	dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
	dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
	dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
	dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
	dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
	dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
	dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
	dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as PreAprobador,
	dbo.fn_hth_obtenerAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Aprobador,
	dbo.fn_hth_obtenerComentarios(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdEstadoOrden),
	au.UserName as UsuarioCreacion,
	ob.FechaCreacion as FechaCreacion,
	ob.UsuarioEdicion,
	ob.FechaEdicion
FROM
	dbo.OrdenBancaria ob
	join Usuario u on ob.IdUsuario = u.IdUsuario and u.Estado = 1
	join dbo.AspNetUsers au on u.IdUsuario = au.IdUsuario
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
	join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
	join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenPreAprobado, @idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
	and (@idSap = '' or ob.IdSap like '%' + @idSap + '%')
	and (@fechaInicio = '' and @fechaFin = '' or ob.FechaOrden >= CAST(@fechaInicio as date) and ob.FechaOrden <= CAST(@fechaFin as date))
GROUP BY
	ob.IdOrdenBancaria,	ob.IdBanco, b.Banco, b.Abreviacion, ob.IdTipoOrden, too.TipoOrden, too.Abreviacion,
	ob.IdEstadoOrden, eo.EstadoOrden, ob.IdSociedad, s.Sociedad, s.Abreviacion, ob.IdSap, ob.Anio, ob.MomentoOrden,
	ob.FechaOrden, ob.NombreArchivo, ob.RutaArchivo, au.UserName, ob.FechaCreacion, ob.UsuarioEdicion, ob.FechaEdicion
HAVING
	(dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario 
	or dbo.fn_hth_obtenerAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario)
ORDER BY
	ob.FechaCreacion DESC;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasAnuladas
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasAnuladas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasAnuladas '000013', 1, 20, '', '', '', 0;

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@pagina int,
@filas int,
@idSociedad nchar(4),
@idBanco nchar(5),
@idTipoOrden nvarchar(5),
@totalRegistros int OUTPUT
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@usuario nvarchar(20) = '',
	@idEstadoOrdenAnulado nchar(2) = 'AN',
	@desde int = 0,
	@hasta int = 0;

SELECT @usuario = UserName FROM dbo.AspNetUsers WHERE IdUsuario = @IdUsuario;
SELECT @desde = CASE @pagina WHEN 1 THEN 1 ELSE ((@pagina - 1) * @filas) + 1 END;
SELECT @hasta = @desde + @filas - 1;

SELECT
	@totalRegistros = COUNT(IdOrdenBancaria)
FROM
	OrdenBancaria
WHERE
	(@idSociedad = '' or IdSociedad = @idSociedad) and (@idBanco = '' or IdBanco = @idBanco) 
	and (@idTipoOrden = '' or IdTipoOrden = @idTipoOrden) and IdEstadoOrden in (@idEstadoOrdenAnulado)
	and dbo.fn_hth_obtenerAnulador(IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden) = @usuario;

WITH ordenBancaria (Id, IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden, 
EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo, 
MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador, PreAprobador, 
Anulador, Comentarios, UsuarioCreacion, FechaCreacion, UsuarioEdicion, FechaEdicion)
AS	
(
	SELECT
		ROW_NUMBER() OVER (ORDER BY ob.FechaCreacion DESC, ob.FechaOrden DESC),
		ob.IdOrdenBancaria,
		b.Banco as Banco,
		b.Abreviacion as BancoCorto,
		ob.IdTipoOrden as IdTipoOrden,
		too.TipoOrden,
		too.Abreviacion as TipoOrdenCorto,
		ob.IdEstadoOrden as IdEstadoOrden,
		eo.EstadoOrden as EstadoOrden,
		ob.IdSociedad as IdSociedad,
		s.Sociedad as Sociedad,
		s.Abreviacion as SociedadCorto,
		ob.IdSap as IdSap,
		ob.Anio as Anio,
		ob.MomentoOrden as MomentoOrden,
		CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
		ob.NombreArchivo as NombreArchivo,
		ob.RutaArchivo,
		dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
		dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
		dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
		dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
		dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
		dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
		dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
		dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as PreAprobador,
		dbo.fn_hth_obtenerAnulador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Aprobador,
		dbo.fn_hth_obtenerComentarios(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdEstadoOrden),
		au.UserName as UsuarioCreacion,
		ob.FechaCreacion as FechaCreacion,
		ob.UsuarioEdicion,
		ob.FechaEdicion
	FROM
		dbo.OrdenBancaria ob
		join Usuario u on ob.IdUsuario = u.IdUsuario and u.Estado = 1
		join dbo.AspNetUsers au on u.IdUsuario = au.IdUsuario
		join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
		join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
		join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
		join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
	WHERE
		ob.IdEstadoOrden in (@idEstadoOrdenAnulado)
		and (@idSociedad = '' or ob.IdSociedad = @idSociedad) and (@idBanco = '' or ob.IdBanco = @idBanco) 
		and (@idTipoOrden = '' or ob.IdTipoOrden = @idTipoOrden)
	GROUP BY
		ob.IdOrdenBancaria,	ob.IdBanco, b.Banco, b.Abreviacion, ob.IdTipoOrden, too.TipoOrden, too.Abreviacion,
		ob.IdEstadoOrden, eo.EstadoOrden, ob.IdSociedad, s.Sociedad, s.Abreviacion, ob.IdSap, ob.Anio, ob.MomentoOrden,
		ob.FechaOrden, ob.NombreArchivo, ob.RutaArchivo, au.UserName, ob.FechaCreacion, ob.UsuarioEdicion, ob.FechaEdicion
	HAVING
		dbo.fn_hth_obtenerAnulador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario
)

SELECT
	IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden, 
	EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo, 
	MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador, PreAprobador, 
	Anulador, Comentarios, UsuarioCreacion, FechaCreacion, UsuarioEdicion, FechaEdicion
FROM
	ordenBancaria
WHERE
	CAST(Id as int) >= @desde and CAST(Id as int) <= @hasta;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasDetalleAnuladas
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasDetalleAnuladas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasDetalleAnuladas '0010', '2200000054', '2018', '20180101';
exec spw_hth_listarOrdenesBancariasDetalleAnuladas '0010', '00001R', '2018', '20181129';
exec spw_hth_listarOrdenesBancariasDetalleAnuladas '0010', 'GL001', '2016', '20160401';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idEstadoOrdenAnulado nchar(2) = 'AN';

SELECT
	obd.IdOrdenBancariaDetalle,
	tt.TipoTransferencia as TipoTransferencia,
	fp.FormaPago as FormaPago,
	stp.SubTipoPago,
	Referencia1 as Referencia1,
	Referencia2 as Referencia2,
	tm1.TipoMoneda as MonedaCargo,
	tm1.Simbolo as MonedaCargoCorto,
	obd.CuentaCargo as CuentaCargo,
	obd.MontoCargo as MontoCargo,
	tm2.TipoMoneda as MonedaAbono,
	tm2.Simbolo as MonedaAbonoCorto,
	obd.CuentaAbono as CuentaAbono,
	obd.CuentaGasto as CuentaGasto,
	obd.MontoAbono as MontoAbono,
	obd.TipoCambio as TipoCambio,
	obd.ModuloRaiz as ModuloRaiz,
	obd.DigitoControl as DigitoControl,
	CASE obd.Indicador WHEN '0' THEN 'MISMA MONEDA' WHEN '1' THEN 'RECALCULO ABONO' WHEN '2' THEN 'RECALCULO CARGO' ELSE '' END as Indicador,
	obd.NroOperacion,
	obd.Beneficiario,
	td.TipoDocumento as TipoDocumento,
	td.Abreviacion as TipoDocumentoCorto,
	obd.NroDocumento,
	obd.Correo,
	obd.NombreBanco,
	obd.RucBanco,
	obd.NroFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFactura, null), 103) as FechaFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFinFactura, null), 103) as FechaFinFactura,
	CASE obd.SignoFactura WHEN '+' THEN 'FACTURA' WHEN '-' THEN 'NOTA CRÉDITO' ELSE '' END as SignoFactura,
	obd.UsuarioCreacion,
	obd.FechaCreacion
FROM
	OrdenBancaria ob
	join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	left join TipoTransferencia tt on obd.IdBanco = tt.IdBanco and obd.IdTipoTransferencia = tt.IdTipoTransferencia and tt.Estado = 1
	left join FormaPago fp on obd.IdBanco = fp.IdBanco and obd.IdFormaPago = fp.IdFormaPago and fp.Estado = 1
	left join SubTipoPago stp on obd.IdBanco = stp.IdBanco and obd.IdSubTipoPago = stp.IdSubTipoPago and stp.Estado = 1
	left join TipoMoneda tm1 on obd.IdBanco = tm1.IdBanco and obd.IdMonedaCargo = tm1.IdTipoMoneda and tm1.Estado = 1
	left join TipoMoneda tm2 on obd.IdBanco = tm2.IdBanco and obd.IdMonedaAbono = tm2.IdTipoMoneda and tm2.Estado = 1
	left join TipoDocumento td on obd.IdBanco = td.IdBanco and obd.IdTipoDocumento = td.IdTipoDocumento and td.Estado = 1
WHERE
	ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
	and ob.idTipoOrden = @idTipoOrden and ob.IdEstadoOrden = @idEstadoOrdenAnulado
ORDER BY
	obd.FechaCreacion ASC;

END
go

CREATE PROCEDURE spw_hth_buscarOrdenesBancariasAnuladas
/*************************************************************************
Nombre: spw_hth_buscarOrdenesBancariasAnuladas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_buscarOrdenesBancariasAnuladas '000002', '2018/01/01', '2018/01/31';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@idSap nvarchar(10),
@fechaInicio nchar(10),
@fechaFin nchar(10)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@usuario nvarchar(20) = '',
	@idEstadoOrdenAnulado nchar(2) = 'AN';

SELECT @usuario = UserName FROM dbo.AspNetUsers WHERE IdUsuario = @IdUsuario;

SELECT
	ob.IdOrdenBancaria,
	b.Banco as Banco,
	b.Abreviacion as BancoCorto,
	ob.IdTipoOrden as IdTipoOrden,
	too.TipoOrden,
	too.Abreviacion as TipoOrdenCorto,
	ob.IdEstadoOrden as IdEstadoOrden,
	eo.EstadoOrden as EstadoOrden,
	ob.IdSociedad as IdSociedad,
	s.Sociedad as Sociedad,
	s.Abreviacion as SociedadCorto,
	ob.IdSap as IdSap,
	ob.Anio as Anio,
	ob.MomentoOrden as MomentoOrden,
	CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
	ob.NombreArchivo as NombreArchivo,
	ob.RutaArchivo,
	dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
	dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
	dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
	dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
	dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
	dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
	dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
	dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as PreAprobador,
	dbo.fn_hth_obtenerAnulador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Aprobador,
	dbo.fn_hth_obtenerComentarios(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdEstadoOrden),
	au.UserName as UsuarioCreacion,
	ob.FechaCreacion as FechaCreacion,
	ob.UsuarioEdicion,
	ob.FechaEdicion
FROM
	dbo.OrdenBancaria ob
	join dbo.AspNetUsers au on ob.IdUsuario = au.IdUsuario
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
	join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
	join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	ob.IdEstadoOrden = @idEstadoOrdenAnulado
	and (@idSap = '' or ob.IdSap like '%' + @idSap + '%')
	and (@fechaInicio = '' and @fechaFin = '' or ob.FechaOrden >= CAST(@fechaInicio as date) and ob.FechaOrden <= CAST(@fechaFin as date))
GROUP BY
	ob.IdOrdenBancaria,	ob.IdBanco, b.Banco, b.Abreviacion, ob.IdTipoOrden, too.TipoOrden, too.Abreviacion,
	ob.IdEstadoOrden, eo.EstadoOrden, ob.IdSociedad, s.Sociedad, s.Abreviacion, ob.IdSap, ob.Anio, ob.MomentoOrden,
	ob.FechaOrden, ob.NombreArchivo, ob.RutaArchivo, au.UserName, ob.FechaCreacion, ob.UsuarioEdicion, ob.FechaEdicion
HAVING
	dbo.fn_hth_obtenerAnulador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario
ORDER BY
	ob.FechaCreacion DESC;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasAprobadasTesoreria
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasAprobadasTesoreria
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasAprobadasTesoreria '000013', 1, 20, '', '', '', 0;

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@pagina int,
@filas int,
@idSociedad nchar(4),
@idBanco nchar(5),
@idTipoOrden nvarchar(5),
@totalRegistros int OUTPUT
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idEstadoOrdenPreAprobado nchar(2) = 'PP',
	@idEstadoOrdenAprobado nchar(2) = 'AP',
	@idEstadoOrdenEntregado nchar(2) = 'ET',
	@idEstadoOrdenPagado nchar(2) = 'PA',
	@idEstadoOrdenConErrores nchar(2) = 'CE',
	@desde int = 0,
	@hasta int = 0;

SELECT @desde = CASE @pagina WHEN 1 THEN 1 ELSE ((@pagina - 1) * @filas) + 1 END;
SELECT @hasta = @desde + @filas - 1;

SELECT
	@totalRegistros = COUNT(IdOrdenBancaria)
FROM
	OrdenBancaria
WHERE
	(@idSociedad = '' or IdSociedad = @idSociedad) and (@idBanco = '' or IdBanco = @idBanco) 
	and (@idTipoOrden = '' or IdTipoOrden = @idTipoOrden)
	and IdEstadoOrden in (@idEstadoOrdenPreAprobado, @idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
	and IdUsuario = @idUsuario;

WITH ordenBancaria (Id, IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden, 
EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo, 
MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador, PreAprobador, 
Aprobador, Comentarios, UsuarioCreacion, FechaCreacion, UsuarioEdicion, FechaEdicion)
AS
(
	SELECT
		ROW_NUMBER() OVER (ORDER BY ob.FechaCreacion DESC, ob.FechaOrden DESC),
		ob.IdOrdenBancaria,
		b.Banco as Banco,
		b.Abreviacion as BancoCorto,
		ob.IdTipoOrden as IdTipoOrden,
		too.TipoOrden,
		too.Abreviacion as TipoOrdenCorto,
		ob.IdEstadoOrden as IdEstadoOrden,
		eo.EstadoOrden as EstadoOrden,
		ob.IdSociedad as IdSociedad,
		s.Sociedad as Sociedad,
		s.Abreviacion as SociedadCorto,
		ob.IdSap as IdSap,
		ob.Anio as Anio,
		ob.MomentoOrden as MomentoOrden,
		CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
		ob.NombreArchivo as NombreArchivo,
		ob.RutaArchivo,
		dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
		dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
		dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
		dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
		dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
		dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
		dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
		dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as PreAprobador,
		dbo.fn_hth_obtenerAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Aprobador,
		dbo.fn_hth_obtenerComentarios(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdEstadoOrden),
		au.UserName as UsuarioCreacion,
		ob.FechaCreacion as FechaCreacion,
		ob.UsuarioEdicion,
		ob.FechaEdicion
	FROM
		dbo.OrdenBancaria ob
		join Usuario u on ob.IdUsuario = u.IdUsuario and u.Estado = 1
		join dbo.AspNetUsers au on u.IdUsuario = au.IdUsuario
		join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
		join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
		join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
		join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
	WHERE
		ob.IdEstadoOrden in (@idEstadoOrdenPreAprobado, @idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
		and (@idSociedad = '' or ob.IdSociedad = @idSociedad) and (@idBanco = '' or ob.IdBanco = @idBanco) 
		and (@idTipoOrden = '' or ob.IdTipoOrden = @idTipoOrden) and ob.IdUsuario = @idUsuario
)

SELECT
	IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden, 
	EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo, 
	MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador, PreAprobador, 
	Aprobador, Comentarios, UsuarioCreacion, FechaCreacion, UsuarioEdicion, FechaEdicion
FROM
	ordenBancaria
WHERE
	CAST(Id as int) >= @desde and CAST(Id as int) <= @hasta;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasDetalleAprobadasTesoreria
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasDetalleAprobadasTesoreria
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasDetalleAprobadasTesoreria '0010', '2200000054', '2018', '20180101';
exec spw_hth_listarOrdenesBancariasDetalleAprobadasTesoreria '0010', '00001R', '2018', '20181129';
exec spw_hth_listarOrdenesBancariasDetalleAprobadasTesoreria '0010', 'GL001', '2016', '20160401';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5)
)
AS
SET NOCOUNT ON;
BEGIN

declare
	@idEstadoOrdenAprobado nchar(2) = 'AP',
	@idEstadoOrdenEntregado nchar(2) = 'ET',
	@idEstadoOrdenPagado nchar(2) = 'PA',
	@idEstadoOrdenConErrores nchar(2) = 'CE';

SELECT
	obd.IdOrdenBancariaDetalle,
	tt.TipoTransferencia as TipoTransferencia,
	fp.FormaPago as FormaPago,
	stp.SubTipoPago,
	Referencia1 as Referencia1,
	Referencia2 as Referencia2,
	tm1.TipoMoneda as MonedaCargo,
	tm1.Simbolo as MonedaCargoCorto,
	obd.CuentaCargo as CuentaCargo,
	obd.MontoCargo as MontoCargo,
	tm2.TipoMoneda as MonedaAbono,
	tm2.Simbolo as MonedaAbonoCorto,
	obd.CuentaAbono as CuentaAbono,
	obd.CuentaGasto as CuentaGasto,
	obd.MontoAbono as MontoAbono,
	obd.TipoCambio as TipoCambio,
	obd.ModuloRaiz as ModuloRaiz,
	obd.DigitoControl as DigitoControl,
	CASE obd.Indicador WHEN '0' THEN 'MISMA MONEDA' WHEN '1' THEN 'RECALCULO ABONO' WHEN '2' THEN 'RECALCULO CARGO' ELSE '' END as Indicador,
	obd.NroOperacion,
	obd.Beneficiario,
	td.TipoDocumento as TipoDocumento,
	td.Abreviacion as TipoDocumentoCorto,
	obd.NroDocumento,
	obd.Correo,
	obd.NombreBanco,
	obd.RucBanco,
	obd.NroFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFactura, null), 103) as FechaFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFinFactura, null), 103) as FechaFinFactura,
	CASE obd.SignoFactura WHEN '+' THEN 'FACTURA' WHEN '-' THEN 'NOTA CRÉDITO' ELSE '' END as SignoFactura,
	obd.IdRespuesta,
	obd.Respuesta,
	obd.NroOrden,
	obd.NroConvenio,
	obd.UsuarioCreacion,
	obd.FechaCreacion
FROM
	OrdenBancaria ob
	join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	left join TipoTransferencia tt on obd.IdBanco = tt.IdBanco and obd.IdTipoTransferencia = tt.IdTipoTransferencia and tt.Estado = 1
	left join FormaPago fp on obd.IdBanco = fp.IdBanco and obd.IdFormaPago = fp.IdFormaPago and fp.Estado = 1
	left join SubTipoPago stp on obd.IdBanco = stp.IdBanco and obd.IdSubTipoPago = stp.IdSubTipoPago and stp.Estado = 1
	left join TipoMoneda tm1 on obd.IdBanco = tm1.IdBanco and obd.IdMonedaCargo = tm1.IdTipoMoneda and tm1.Estado = 1
	left join TipoMoneda tm2 on obd.IdBanco = tm2.IdBanco and obd.IdMonedaAbono = tm2.IdTipoMoneda and tm2.Estado = 1
	left join TipoDocumento td on obd.IdBanco = td.IdBanco and obd.IdTipoDocumento = td.IdTipoDocumento and td.Estado = 1
WHERE
	ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
	and ob.IdTipoOrden = @idTipoOrden and ob.IdEstadoOrden 
	in (@idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
ORDER BY
	ob.FechaCreacion ASC;

END
go

CREATE PROCEDURE spw_hth_buscarOrdenesBancariasAprobadasTesoreria
/*************************************************************************
Nombre: spw_hth_buscarOrdenesBancariasAprobadasTesoreria
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_buscarOrdenesBancariasAprobadasTesoreria '000013', '2018/01/01', '2018/01/31';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@idSap nvarchar(10),
@fechaInicio nchar(10),
@fechaFin nchar(10)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idEstadoOrdenAprobado nchar(2) = 'AP',
	@idEstadoOrdenEntregado nchar(2) = 'ET',
	@idEstadoOrdenPagado nchar(2) = 'PA',
	@idEstadoOrdenConErrores nchar(2) = 'CE';

SELECT
	ob.IdOrdenBancaria,
	b.Banco as Banco,
	b.Abreviacion as BancoCorto,
	ob.IdTipoOrden as IdTipoOrden,
	too.TipoOrden,
	too.Abreviacion as TipoOrdenCorto,
	ob.IdEstadoOrden as IdEstadoOrden,
	eo.EstadoOrden as EstadoOrden,
	ob.IdSociedad as IdSociedad,
	s.Sociedad as Sociedad,
	s.Abreviacion as SociedadCorto,
	ob.IdSap as IdSap,
	ob.Anio as Anio,
	ob.MomentoOrden as MomentoOrden,
	CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
	ob.NombreArchivo as NombreArchivo,
	ob.RutaArchivo,
	dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
	dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
	dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
	dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
	dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
	dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
	dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
	dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as PreAprobador,
	dbo.fn_hth_obtenerAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Aprobador,
	dbo.fn_hth_obtenerComentarios(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdEstadoOrden),
	au.UserName as UsuarioCreacion,
	ob.FechaCreacion as FechaCreacion,
	ob.UsuarioEdicion,
	ob.FechaEdicion
FROM
	dbo.OrdenBancaria ob
	join Usuario u on ob.IdUsuario = u.IdUsuario and u.Estado = 1
	join dbo.AspNetUsers au on u.IdUsuario = au.IdUsuario
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
	join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
	join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
	and (@idSap = '' or ob.IdSap like '%' + @idSap + '%')
	and (@fechaInicio = '' and @fechaFin = '' or ob.FechaOrden >= CAST(@fechaInicio as date) and ob.FechaOrden <= CAST(@fechaFin as date))
	and ob.IdUsuario = @idUsuario
ORDER BY
	ob.FechaCreacion DESC;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasDiarias
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasDiarias
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasDiarias '000001', 1, 20, '', '', '', '', 0;

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@pagina int,
@filas int,
@idSociedad nchar(4),
@idBanco nchar(5),
@idTipoOrden nvarchar(5),
@idEstadoOrden nchar(2),
@totalRegistros int OUTPUT
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@desde int = 0,
	@hasta int = 0;

SELECT @desde = CASE @pagina WHEN 1 THEN 1 ELSE ((@pagina - 1) * @filas) + 1 END;
SELECT @hasta = @desde + @filas - 1;

SELECT
	@totalRegistros = COUNT(IdOrdenBancaria)
FROM
	OrdenBancaria
WHERE
	(@idSociedad = '' or IdSociedad = @idSociedad) and (@idBanco = '' or IdBanco = @idBanco) 
	and (@idTipoOrden = '' or IdTipoOrden = @idTipoOrden) and (@idEstadoOrden = '' or IdEstadoOrden = @idEstadoOrden)
	and IdUsuario = @idUsuario and CAST(FechaCreacion as date) = CAST(GETDATE() as date);

WITH ordenBancaria (Id, IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden, 
EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo, 
MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador, PreAprobador, 
Aprobador, Comentarios, UsuarioCreacion, FechaCreacion, UsuarioEdicion, FechaEdicion)
AS
(
	SELECT
		ROW_NUMBER() OVER (ORDER BY ob.FechaCreacion DESC, ob.FechaOrden DESC),
		ob.IdOrdenBancaria,
		b.Banco as Banco,
		b.Abreviacion as BancoCorto,
		ob.IdTipoOrden as IdTipoOrden,
		too.TipoOrden,
		too.Abreviacion as TipoOrdenCorto,
		ob.IdEstadoOrden as IdEstadoOrden,
		eo.EstadoOrden as EstadoOrden,
		ob.IdSociedad as IdSociedad,
		s.Sociedad as Sociedad,
		s.Abreviacion as SociedadCorto,
		ob.IdSap as IdSap,
		ob.Anio as Anio,
		ob.MomentoOrden as MomentoOrden,
		CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
		ob.NombreArchivo as NombreArchivo,
		ob.RutaArchivo,
		dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
		dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
		dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
		dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
		dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
		dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
		dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
		dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as PreAprobador,
		dbo.fn_hth_obtenerAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Aprobador,
		dbo.fn_hth_obtenerComentarios(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdEstadoOrden),
		au.UserName as UsuarioCreacion,
		ob.FechaCreacion as FechaCreacion,
		ob.UsuarioEdicion,
		ob.FechaEdicion
	FROM
		dbo.OrdenBancaria ob
		join Usuario u on ob.IdUsuario = u.IdUsuario and u.Estado = 1
		join dbo.AspNetUsers au on u.IdUsuario = au.IdUsuario
		join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
		join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
		join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
		join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
	WHERE
		(@idSociedad = '' or ob.IdSociedad = @idSociedad) and (@idBanco = '' or ob.IdBanco = @idBanco) 
		and (@idTipoOrden = '' or ob.IdTipoOrden = @idTipoOrden) and (@idEstadoOrden = '' or ob.IdEstadoOrden = @idEstadoOrden)
		and ob.IdUsuario = @idUsuario and CAST(ob.FechaCreacion as date) = CAST(GETDATE() as date)
)

SELECT
	IdOrdenBancaria, Banco, BancoCorto, IdTipoOrden, TipoOrden, TipoOrdenCorto, IdEstadoOrden, 
	EstadoOrden, IdSociedad, Sociedad, SociedadCorto, IdSap, Anio, MomentoOrden, FechaOrden, NombreArchivo, RutaArchivo, 
	MonedaLocal, MonedaLocalCorto, ImporteLocal, MonedaForanea, MonedaForaneaCorto, ImporteForanea, Liberador, PreAprobador, 
	Aprobador, Comentarios, UsuarioCreacion, FechaCreacion, UsuarioEdicion, FechaEdicion
FROM
	ordenBancaria
WHERE
	CAST(Id as int) >= @desde and CAST(Id as int) <= @hasta;

END
go

CREATE PROCEDURE spw_hth_listarOrdenesBancariasDetalleDiarias
/*************************************************************************
Nombre: spw_hth_listarOrdenesBancariasDetalleDiarias
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarOrdenesBancariasDetalleDiarias '0010', '2200000054', '2018', '20180101';
exec spw_hth_listarOrdenesBancariasDetalleDiarias '0010', '00001R', '2018', '20181129';
exec spw_hth_listarOrdenesBancariasDetalleDiarias '0010', 'GL001', '2016', '20160401';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5)
)
AS
SET NOCOUNT ON;
BEGIN

SELECT
	obd.IdOrdenBancariaDetalle,
	tt.TipoTransferencia as TipoTransferencia,
	fp.FormaPago as FormaPago,
	stp.SubTipoPago,
	Referencia1 as Referencia1,
	Referencia2 as Referencia2,
	tm1.TipoMoneda as MonedaCargo,
	tm1.Simbolo as MonedaCargoCorto,
	obd.CuentaCargo as CuentaCargo,
	obd.MontoCargo as MontoCargo,
	tm2.TipoMoneda as MonedaAbono,
	tm2.Simbolo as MonedaAbonoCorto,
	obd.CuentaAbono as CuentaAbono,
	obd.CuentaGasto as CuentaGasto,
	obd.MontoAbono as MontoAbono,
	obd.TipoCambio as TipoCambio,
	obd.ModuloRaiz as ModuloRaiz,
	obd.DigitoControl as DigitoControl,
	CASE obd.Indicador WHEN '0' THEN 'MISMA MONEDA' WHEN '1' THEN 'RECALCULO ABONO' WHEN '2' THEN 'RECALCULO CARGO' ELSE '' END as Indicador,
	obd.NroOperacion,
	obd.Beneficiario,
	td.TipoDocumento as TipoDocumento,
	td.Abreviacion as TipoDocumentoCorto,
	obd.NroDocumento,
	obd.Correo,
	obd.NombreBanco,
	obd.RucBanco,
	obd.NroFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFactura, null), 103) as FechaFactura,
	CONVERT(nvarchar(10), ISNULL(obd.FechaFinFactura, null), 103) as FechaFinFactura,
	CASE obd.SignoFactura WHEN '+' THEN 'FACTURA' WHEN '-' THEN 'NOTA CRÉDITO' ELSE '' END as SignoFactura,
	obd.IdRespuesta,
	obd.Respuesta,
	obd.NroOrden,
	obd.NroConvenio,
	obd.UsuarioCreacion,
	obd.FechaCreacion
FROM
	OrdenBancaria ob
	join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	left join TipoTransferencia tt on obd.IdBanco = tt.IdBanco and obd.IdTipoTransferencia = tt.IdTipoTransferencia and tt.Estado = 1
	left join FormaPago fp on obd.IdBanco = fp.IdBanco and obd.IdFormaPago = fp.IdFormaPago and fp.Estado = 1
	left join SubTipoPago stp on obd.IdBanco = stp.IdBanco and obd.IdSubTipoPago = stp.IdSubTipoPago and stp.Estado = 1
	left join TipoMoneda tm1 on obd.IdBanco = tm1.IdBanco and obd.IdMonedaCargo = tm1.IdTipoMoneda and tm1.Estado = 1
	left join TipoMoneda tm2 on obd.IdBanco = tm2.IdBanco and obd.IdMonedaAbono = tm2.IdTipoMoneda and tm2.Estado = 1
	left join TipoDocumento td on obd.IdBanco = td.IdBanco and obd.IdTipoDocumento = td.IdTipoDocumento and td.Estado = 1
WHERE
	ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
	and ob.IdTipoOrden = @idTipoOrden and CAST(ob.FechaCreacion as date) = CAST(GETDATE() as date)
ORDER BY
	ob.FechaCreacion ASC;

END
go

CREATE PROCEDURE spw_hth_buscarOrdenesBancariasDiarias
/*************************************************************************
Nombre: spw_hth_buscarOrdenesBancariasDiarias
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_buscarOrdenesBancariasDiarias '000013', '2018/01/01', '2018/01/31';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@idSap nvarchar(10)
)
AS
BEGIN
SET NOCOUNT ON;

SELECT
	ob.IdOrdenBancaria,
	b.Banco as Banco,
	b.Abreviacion as BancoCorto,
	ob.IdTipoOrden as IdTipoOrden,
	too.TipoOrden,
	too.Abreviacion as TipoOrdenCorto,
	ob.IdEstadoOrden as IdEstadoOrden,
	eo.EstadoOrden as EstadoOrden,
	ob.IdSociedad as IdSociedad,
	s.Sociedad as Sociedad,
	s.Abreviacion as SociedadCorto,
	ob.IdSap as IdSap,
	ob.Anio as Anio,
	ob.MomentoOrden as MomentoOrden,
	CONVERT(nvarchar(10), ISNULL(ob.FechaOrden, null), 103) as FechaOrden,
	ob.NombreArchivo as NombreArchivo,
	ob.RutaArchivo,
	dbo.fn_hth_obtenerMonedaLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaLocal,
	dbo.fn_hth_obtenerMonedaSimboloLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoLocal,
	dbo.fn_hth_obtenerImporteLocal(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteLocal,
	dbo.fn_hth_obtenerMonedaForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaForanea,
	dbo.fn_hth_obtenerMonedaSimboloForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as MonedaCortoForanea,
	dbo.fn_hth_obtenerImporteForanea(ob.IdBanco, ob.IdTipoOrden, ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden) as ImporteForanea,
	dbo.fn_hth_obtenerLiberador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Liberador,
	dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as PreAprobador,
	dbo.fn_hth_obtenerAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) as Aprobador,
	dbo.fn_hth_obtenerComentarios(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdEstadoOrden),
	au.UserName as UsuarioCreacion,
	ob.FechaCreacion as FechaCreacion,
	ob.UsuarioEdicion,
	ob.FechaEdicion
FROM
	dbo.OrdenBancaria ob
	join Usuario u on ob.IdUsuario = u.IdUsuario and u.Estado = 1
	join dbo.AspNetUsers au on u.IdUsuario = au.IdUsuario
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
	join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
	join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	CAST(ob.FechaCreacion as date) = CAST(GETDATE() as date)
	and (@idSap = '' or ob.IdSap like '%' + @idSap + '%')
	and ob.IdUsuario = @idUsuario
ORDER BY
	ob.FechaCreacion DESC;

END
go

CREATE FUNCTION fn_hth_obtenerMonedaLocal
/*************************************************************************
Nombre: fn_hth_obtenerMonedaLocal
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
SELECT dbo.fn_hth_obtenerMonedaLocal ('00WIE', '06', '0010', '2200000054', '2018', '20180101');
SELECT dbo.fn_hth_obtenerMonedaLocal ('00WIE', '07', '0010', '00001R', '2018', '20181129');
SELECT dbo.fn_hth_obtenerMonedaLocal ('00WIE', '01', '0010', 'GL001', '2016', '20160401');
SELECT dbo.fn_hth_obtenerMonedaLocal ('00WIE', '03', '0010', 'GL001', '2016', '20160401');

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idBanco nchar(5),
@idTipoOrden nvarchar(5),
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8)
)
RETURNS nvarchar(50)
AS
BEGIN

declare
	@moneda nvarchar(50),
	@idTipoOrdenTransferencia nvarchar(5) = '06',
	@idTipoOrdenTransferenciaBcr nvarchar(5) = '07',
	@idTipoOrdenProveedor nvarchar(5) = '01',
	@idTipoOrdenCamaraComercio nvarchar(5) = '03',
	@idBancoScotiabank nchar(5) = '00WIE',	
	@idTipoMoneda nvarchar(5) = '',
	@tipoMonedaSoles nvarchar(50) = 'Soles',
	@idTipoMonedaSoles nvarchar(5) = '00';

IF (@idTipoOrden = @idTipoOrdenTransferencia)
BEGIN
	SELECT
		@idTipoMoneda = obd.IdMonedaAbono
	FROM
		OrdenBancaria ob
		join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
		join TipoMoneda tm on obd.IdMonedaAbono = tm.IdTipoMoneda and tm.Estado = 1
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
		and ob.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaSoles;

END
ELSE IF (@idTipoOrden = @idTipoOrdenTransferenciaBcr)
BEGIN
	SELECT
		@idTipoMoneda = obd.IdMonedaCargo
	FROM
		OrdenBancaria ob
		join 
		(
		SELECT TOP 1 obd.IdSociedad, obd.IdSap, obd.Anio, obd.MomentoOrden, obd.IdTipoOrden, obd.IdMonedaCargo
		FROM OrdenBancariaDetalle obd
		join TipoMoneda tm on obd.IdMonedaCargo = tm.IdTipoMoneda and tm.Estado = 1
		WHERE obd.IdSociedad = @idSociedad and obd.IdSap = @idSap and obd.Anio = @anio
		and obd.MomentoOrden = @momentoOrden and obd.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaSoles
		) obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden and ob.IdTipoOrden = @idTipoOrden;
END
ELSE IF (@idTipoOrden = @idTipoOrdenProveedor or @idTipoOrden = @idTipoOrdenCamaraComercio)
BEGIN
	SELECT
		@idTipoMoneda = obd.IdMonedaAbono
	FROM
		OrdenBancaria ob
		join 
		(
		SELECT TOP 1 obd.IdSociedad, obd.IdSap, obd.Anio, obd.MomentoOrden, obd.IdTipoOrden, obd.IdMonedaAbono
		FROM OrdenBancariaDetalle obd
		join TipoMoneda tm on obd.IdMonedaAbono = tm.IdTipoMoneda and tm.Estado = 1
		WHERE obd.IdSociedad = @idSociedad and obd.IdSap = @idSap and obd.Anio = @anio
		and obd.MomentoOrden = @momentoOrden and obd.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaSoles
		) obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden and ob.IdTipoOrden = @idTipoOrden;
END

IF (@idBanco = @idBancoScotiabank)
BEGIN
	if (@idTipoMoneda = @idTipoMonedaSoles)
	begin
		SELECT @moneda = 'PEN';
	end
END

return @moneda;

END
go

CREATE FUNCTION fn_hth_obtenerMonedaSimboloLocal
/*************************************************************************
Nombre: fn_hth_obtenerMonedaSimbolo
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
SELECT dbo.fn_hth_obtenerMonedaSimboloLocal ('00WIE', '06', '0010', '2200000054', '2018', '20180101');
SELECT dbo.fn_hth_obtenerMonedaSimboloLocal ('00WIE', '07', '0010', '00001R', '2018', '20181129');
SELECT dbo.fn_hth_obtenerMonedaSimboloLocal ('00WIE', '01', '0010', 'GL001', '2016', '20160401');

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idBanco nchar(5),
@idTipoOrden nvarchar(5),
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8)
)
RETURNS nvarchar(50)
AS
BEGIN

declare
	@simbolo nvarchar(50) = '',
	@idTipoOrdenTransferencia nvarchar(5) = '06',
	@idTipoOrdenTransferenciaBcr nvarchar(5) = '07',
	@idTipoOrdenProveedor nvarchar(5) = '01',
	@idTipoOrdenCamaraComercio nvarchar(5) = '03',
	@tipoMonedaSoles nvarchar(50) = 'Soles';

IF (@idTipoOrden = @idTipoOrdenTransferencia)
BEGIN
	SELECT
		@simbolo = tm.TipoMoneda + ' ' + tm.Simbolo
	FROM
		OrdenBancaria ob
		join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
		join TipoMoneda tm on obd.IdMonedaAbono = tm.IdTipoMoneda and tm.Estado = 1
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
		and ob.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaSoles;

END
ELSE IF (@idTipoOrden = @idTipoOrdenTransferenciaBcr)
BEGIN
	SELECT
		@simbolo = obd.TipoMoneda + ' ' + obd.Simbolo
	FROM
		OrdenBancaria ob
		join 
		(
		SELECT TOP 1 obd.IdSociedad, obd.IdSap, obd.Anio, obd.MomentoOrden, obd.IdMonedaCargo, obd.IdTipoOrden, tm.TipoMoneda, tm.Simbolo
		FROM OrdenBancariaDetalle obd
		join TipoMoneda tm on obd.IdMonedaCargo = tm.IdTipoMoneda and tm.Estado = 1
		WHERE obd.IdSociedad = @idSociedad and obd.IdSap = @idSap and obd.Anio = @anio
		and obd.MomentoOrden = @momentoOrden and obd.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaSoles
		) obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden and ob.IdTipoOrden = @idTipoOrden;
END
ELSE IF (@idTipoOrden = @idTipoOrdenProveedor or @idTipoOrden = @idTipoOrdenCamaraComercio)
BEGIN
	SELECT
		@simbolo = obd.TipoMoneda + ' ' + obd.Simbolo
	FROM
		OrdenBancaria ob
		join 
		(
		SELECT TOP 1 obd.IdSociedad, obd.IdSap, obd.Anio, obd.MomentoOrden, obd.IdMonedaAbono, obd.IdTipoOrden, tm.TipoMoneda, tm.Simbolo
		FROM OrdenBancariaDetalle obd
		join TipoMoneda tm on obd.IdMonedaAbono = tm.IdTipoMoneda and tm.Estado = 1
		WHERE obd.IdSociedad = @idSociedad and obd.IdSap = @idSap and obd.Anio = @anio
		and obd.MomentoOrden = @momentoOrden and obd.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaSoles
		) obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden and ob.IdTipoOrden = @idTipoOrden;
END

return @simbolo;

END
go

CREATE FUNCTION fn_hth_obtenerImporteLocal
/*************************************************************************
Nombre: fn_hth_obtenerImporteLocal
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
SELECT dbo.fn_hth_obtenerImporteLocal ('00WIE', '06', '0010', '2200000054', '2018', '20180101');
SELECT dbo.fn_hth_obtenerImporteLocal ('00WIE', '07', '0010', '00001R', '2018', '20181129');
SELECT dbo.fn_hth_obtenerImporteLocal ('00WIE', '01', '0010', 'GL001', '2016', '20160401');

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idBanco nchar(5),
@idTipoOrden nchar(2),
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8)
)
RETURNS decimal(12, 2)
AS
BEGIN

declare
	@importe decimal(12, 2),
	@idTipoOrdenTransferencia nvarchar(5) = '06',
	@idTipoOrdenTransferenciaBcr nvarchar(5) = '07',
	@idTipoOrdenProveedor nvarchar(5) = '01',
	@idTipoOrdenCamaraComercio nvarchar(5) = '03',
	@tipoMonedaSoles nvarchar(50) = 'Soles';

IF (@idTipoOrden = @idTipoOrdenTransferencia)
BEGIN
	SELECT
		@importe = obd.MontoAbono
	FROM
		OrdenBancaria ob
		join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
		join TipoMoneda tm on obd.IdMonedaAbono = tm.IdTipoMoneda and tm.Estado = 1
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
		and ob.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaSoles;

END
ELSE IF (@idTipoOrden = @idTipoOrdenTransferenciaBcr)
BEGIN
	SELECT
		@importe = SUM(obd.MontoCargo)
	FROM
		OrdenBancaria ob
		join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
		join TipoMoneda tm on obd.IdMonedaCargo = tm.IdTipoMoneda and tm.Estado = 1
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
		and ob.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaSoles;
END
ELSE IF (@idTipoOrden = @idTipoOrdenProveedor or @idTipoOrden = @idTipoOrdenCamaraComercio)
BEGIN
	SELECT
		@importe = SUM(obd.MontoAbono)
	FROM
		OrdenBancaria ob
		join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
		join TipoMoneda tm on obd.IdMonedaAbono = tm.IdTipoMoneda and tm.Estado = 1
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
		and ob.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaSoles;
END

return @importe;

END
go

CREATE FUNCTION fn_hth_obtenerMonedaForanea
/*************************************************************************
Nombre: fn_hth_obtenerMonedaForanea
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
SELECT dbo.fn_hth_obtenerMonedaForanea ('00WIE', '06', '0010', '2200000054', '2018', '20180101');
SELECT dbo.fn_hth_obtenerMonedaForanea ('00WIE', '07', '0010', '00001R', '2018', '20181129');
SELECT dbo.fn_hth_obtenerMonedaForanea ('00WIE', '01', '0010', 'GL001', '2016', '20160401');

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idBanco nchar(5),
@idTipoOrden nchar(2),
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8)
)
RETURNS nvarchar(50)
AS
BEGIN

declare
	@moneda nvarchar(50),
	@idTipoOrdenTransferencia nvarchar(5) = '06',
	@idTipoOrdenTransferenciaBcr nvarchar(5) = '07',
	@idTipoOrdenProveedor nvarchar(5) = '01',
	@idTipoOrdenCamaraComercio nvarchar(5) = '03',
	@idBancoScotiabank nchar(5) = '00WIE',
	@idTipoMoneda nvarchar(5) = '',
	@tipoMonedaDolares nvarchar(50) = 'Dólares',
	@idTipoMonedaDolares nvarchar(5) = '01';

IF (@idTipoOrden = @idTipoOrdenTransferencia)
BEGIN
	SELECT
		@idTipoMoneda = obd.IdMonedaAbono
	FROM
		OrdenBancaria ob
		join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
		join TipoMoneda tm on obd.IdMonedaAbono = tm.IdTipoMoneda and tm.Estado = 1
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
		and ob.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaDolares;

END
ELSE IF (@idTipoOrden = @idTipoOrdenTransferenciaBcr)
BEGIN
	SELECT
		@idTipoMoneda = obd.IdMonedaCargo
	FROM
		OrdenBancaria ob
		join 
		(
		SELECT TOP 1 obd.IdSociedad, obd.IdSap, obd.Anio, obd.MomentoOrden, obd.IdTipoOrden, obd.IdMonedaCargo
		FROM OrdenBancariaDetalle obd
		join TipoMoneda tm on obd.IdMonedaCargo = tm.IdTipoMoneda and tm.Estado = 1
		WHERE obd.IdSociedad = @idSociedad and obd.IdSap = @idSap and obd.Anio = @anio
		and obd.MomentoOrden = @momentoOrden and obd.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaDolares
		) obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden and ob.IdTipoOrden = @idTipoOrden;
END
ELSE IF (@idTipoOrden = @idTipoOrdenProveedor or @idTipoOrden = @idTipoOrdenCamaraComercio)
BEGIN
	SELECT
		@idTipoMoneda = obd.IdMonedaAbono
	FROM
		OrdenBancaria ob
		join 
		(
		SELECT TOP 1 obd.IdSociedad, obd.IdSap, obd.Anio, obd.MomentoOrden, obd.IdTipoOrden, obd.IdMonedaAbono
		FROM OrdenBancariaDetalle obd
		join TipoMoneda tm on obd.IdMonedaAbono = tm.IdTipoMoneda and tm.Estado = 1
		WHERE obd.IdSociedad = @idSociedad and obd.IdSap = @idSap and obd.Anio = @anio
		and obd.MomentoOrden = @momentoOrden and obd.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaDolares
		) obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden and ob.IdTipoOrden = @idTipoOrden;
END

IF (@idBanco = @idBancoScotiabank)
BEGIN
	if (@idTipoMoneda = @idTipoMonedaDolares)
	begin
		SELECT @moneda = 'USD';
	end
END

return @moneda;

END
go

CREATE FUNCTION fn_hth_obtenerMonedaSimboloForanea
/*************************************************************************
Nombre: fn_hth_obtenerMonedaSimboloForanea
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
SELECT dbo.fn_hth_obtenerMonedaSimboloForanea ('00WIE', '06', '0010', '2200000054', '2018', '20180101');
SELECT dbo.fn_hth_obtenerMonedaSimboloForanea ('00WIE', '07', '0010', '00001R', '2018', '20181129');
SELECT dbo.fn_hth_obtenerMonedaSimboloForanea ('00WIE', '01', '0010', 'GL001', '2016', '20160401');

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idBanco nchar(5),
@idTipoOrden nchar(2),
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8)
)
RETURNS nvarchar(50)
AS
BEGIN

declare
	@simbolo nvarchar(50),
	@idTipoOrdenTransferencia nvarchar(5) = '06',
	@idTipoOrdenTransferenciaBcr nvarchar(5) = '07',
	@idTipoOrdenProveedor nvarchar(5) = '01',
	@idTipoOrdenCamaraComercio nvarchar(5) = '03',
	@tipoMonedaDolares nvarchar(50) = 'Dólares';

IF (@idTipoOrden = @idTipoOrdenTransferencia)
BEGIN
	SELECT
		@simbolo = tm.TipoMoneda + ' ' + tm.Simbolo
	FROM
		OrdenBancaria ob
		join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
		join TipoMoneda tm on obd.IdMonedaAbono = tm.IdTipoMoneda and tm.Estado = 1
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
		and ob.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaDolares;

END
ELSE IF (@idTipoOrden = @idTipoOrdenTransferenciaBcr)
BEGIN
	SELECT
		@simbolo = obd.TipoMoneda + ' ' + obd.Simbolo
	FROM
		OrdenBancaria ob
		join 
		(
		SELECT TOP 1 obd.IdSociedad, obd.IdSap, obd.Anio, obd.MomentoOrden, obd.IdTipoOrden, obd.IdMonedaCargo, tm.TipoMoneda, tm.Simbolo
		FROM OrdenBancariaDetalle obd
		join TipoMoneda tm on obd.IdMonedaCargo = tm.IdTipoMoneda and tm.Estado = 1
		WHERE obd.IdSociedad = @idSociedad and obd.IdSap = @idSap and obd.Anio = @anio and obd.MomentoOrden = @momentoOrden
		and obd.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaDolares
		) obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden and ob.IdTipoOrden = @idTipoOrden;
END
ELSE IF (@idTipoOrden = @idTipoOrdenProveedor or @idTipoOrden = @idTipoOrdenCamaraComercio)
BEGIN
	SELECT
		@simbolo = obd.TipoMoneda + ' ' + obd.Simbolo
	FROM
		OrdenBancaria ob
		join 
		(
		SELECT TOP 1 obd.IdSociedad, obd.IdSap, obd.Anio, obd.MomentoOrden, obd.IdTipoOrden, obd.IdMonedaAbono, tm.TipoMoneda, tm.Simbolo
		FROM OrdenBancariaDetalle obd
		join TipoMoneda tm on obd.IdMonedaAbono = tm.IdTipoMoneda and tm.Estado = 1
		WHERE obd.IdSociedad = @idSociedad and obd.IdSap = @idSap and obd.Anio = @anio and obd.MomentoOrden = @momentoOrden and obd.IdTipoOrden = @idTipoOrden
		and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaDolares
		) obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden and ob.IdTipoOrden = @idTipoOrden;
END

return @simbolo;

END
go

CREATE FUNCTION fn_hth_obtenerImporteForanea
/*************************************************************************
Nombre: fn_hth_obtenerImporteForanea
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
SELECT dbo.fn_hth_obtenerImporteForanea ('00WIE', '06', '0010', '2200000054', '2018', '20180101');
SELECT dbo.fn_hth_obtenerImporteForanea ('00WIE', '07', '0010', '00001R', '2018', '20181129');
SELECT dbo.fn_hth_obtenerImporteForanea ('00WIE', '01', '0010', 'GL001', '2016', '20160401');

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idBanco nchar(5),
@idTipoOrden nchar(2),
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8)
)
RETURNS decimal(12, 2)
AS
BEGIN

declare
	@importe decimal(12, 2),
	@idTipoOrdenTransferencia nvarchar(5) = '06',
	@idTipoOrdenTransferenciaBcr nvarchar(5) = '07',
	@idTipoOrdenProveedor nvarchar(5) = '01',
	@idTipoOrdenCamaraComercio nvarchar(5) = '03',
	@tipoMonedaDolares nvarchar(50) = 'Dólares';

IF (@idTipoOrden = @idTipoOrdenTransferencia)
BEGIN
	SELECT
		@importe = obd.MontoAbono
	FROM
		OrdenBancaria ob
		join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
		join TipoMoneda tm on obd.IdMonedaAbono = tm.IdTipoMoneda and tm.Estado = 1
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
		and ob.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaDolares;

END
ELSE IF (@idTipoOrden = @idTipoOrdenTransferenciaBcr)
BEGIN
	SELECT
		@importe = SUM(obd.MontoCargo)
	FROM
		OrdenBancaria ob
		join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
		join TipoMoneda tm on obd.IdMonedaCargo = tm.IdTipoMoneda and tm.Estado = 1
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
		and ob.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaDolares;
END
ELSE IF (@idTipoOrden = @idTipoOrdenProveedor or @idTipoOrden = @idTipoOrdenCamaraComercio)
BEGIN
	SELECT
		@importe = SUM(obd.MontoAbono)
	FROM
		OrdenBancaria ob
		join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
		join TipoMoneda tm on obd.IdMonedaAbono = tm.IdTipoMoneda and tm.Estado = 1
	WHERE
		ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden
		and ob.IdTipoOrden = @idTipoOrden and tm.IdBanco = @idBanco and tm.TipoMoneda = @tipoMonedaDolares;
END

return @importe;

END
go

CREATE FUNCTION fn_hth_obtenerLiberador
/*************************************************************************
Nombre: fn_hth_obtenerLiberador
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
SELECT dbo.fn_hth_obtenerLiberador ('0010', '2200000054', '2018', '20180101');

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5)
)
RETURNS nvarchar(20)
BEGIN

declare
	@liberador nvarchar(20) = '',
	@idEstadoFlujoLiberado nchar(3) = '001';

SELECT
	@liberador = au.UserName
FROM
	FlujoAprobacion fa
	join dbo.AspNetUsers au on fa.IdUsuario = au.IdUsuario
WHERE
	IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden
	and IdTipoOrden = @idTipoOrden and IdEstadoFlujo = @idEstadoFlujoLiberado;

return @liberador;

END
go

CREATE FUNCTION fn_hth_obtenerPreAprobador
/*************************************************************************
Nombre: fn_hth_obtenerPreAprobador
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
SELECT dbo.fn_hth_obtenerPreAprobador ('0010', '2200000054', '2018', '20180101');

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5)
)
RETURNS nvarchar(20)
BEGIN

declare
	@preAprobador nvarchar(20) = '',
	@idEstadoFlujoPreAprobado nchar(3) = '003';

SELECT
	@preAprobador = au.UserName
FROM
	FlujoAprobacion fa
	join dbo.AspNetUsers au on fa.IdUsuario = au.IdUsuario
WHERE
	IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden
	and IdTipoOrden = @idTipoOrden and IdEstadoFlujo = @idEstadoFlujoPreAprobado;

return @preAprobador;

END
go

CREATE FUNCTION fn_hth_obtenerAprobador
/*************************************************************************
Nombre: fn_hth_obtenerAprobador
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
SELECT dbo.fn_hth_obtenerAprobador ('0010', '2200000054', '2018', '20180101');

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5)
)
RETURNS nvarchar(20)
BEGIN

declare
	@aprobador nvarchar(20) = '',
	@idEstadoFlujoAprobado nchar(3) = '005';

SELECT
	@aprobador = au.UserName
FROM
	FlujoAprobacion fa
	join dbo.AspNetUsers au on fa.IdUsuario = au.IdUsuario
WHERE
	IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden
	and IdTipoOrden = @idTipoOrden and IdEstadoFlujo = @idEstadoFlujoAprobado;

return @aprobador;

END
go

CREATE FUNCTION fn_hth_obtenerAnulador
/*************************************************************************
Nombre: fn_hth_obtenerAnulador
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
SELECT dbo.fn_hth_obtenerAnulador ('0010', '2200000054', '2018', '20180101');

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5)
)
RETURNS nvarchar(20)
BEGIN

declare
	@anulador nvarchar(20) = '',
	@idEstadoFlujoAnulado nchar(3) = '004';

SELECT
	@anulador = au.UserName
FROM
	FlujoAprobacion fa
	join dbo.AspNetUsers au on fa.IdUsuario = au.IdUsuario
WHERE
	IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden
	and IdTipoOrden = @idTipoOrden and IdEstadoFlujo = @idEstadoFlujoAnulado;

return @anulador;

END
go

CREATE FUNCTION fn_hth_obtenerComentarios
/*************************************************************************
Nombre: fn_hth_obtenerComentarios
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
SELECT dbo.fn_hth_obtenerComentarios ('0010', '2200000054', '2018', '20180101', 'LI');

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5),
@idEstadoOrden nchar(2)
)
RETURNS nvarchar(300)
BEGIN

declare
	@comentarios nvarchar(300) = '',
	@idEstadoOrdenLiberado nchar(2) = 'LI',
	@idEstadoOrdenDeshecho nchar(2) = 'DE',
	@idEstadoOrdenPreAprobado nchar(2) = 'PP',
	@idEstadoOrdenAnulado nchar(2) = 'AN',
	@idEstadoOrdenAprobado nchar(2) = 'AP',
	@idEstadoOrdenEntregado nchar(2) = 'ET',
	@idEstadoOrdenPagado nchar(2) = 'PA',
	@idEstadoOrdenConErrores nchar(2) = 'CE',
	@idEstadoFlujoLiberado nchar(3) = '001',
	@idEstadoFlujoDeshecho nchar(3) = '002',
	@idEstadoFlujoPreAprobado nchar(3) = '003',
	@idEstadoFlujoAnulado nchar(3) = '004',
	@idEstadoFlujoAprobado nchar(3) = '005',
	@idEstadoFlujo nchar(3) = '';

if (@idEstadoOrden = @idEstadoOrdenLiberado)
	SET @idEstadoFlujo = @idEstadoFlujoLiberado;
if (@idEstadoOrden = @idEstadoOrdenDeshecho)
	SET @idEstadoFlujo = @idEstadoFlujoDeshecho;
if (@idEstadoOrden = @idEstadoOrdenPreAprobado)
	SET @idEstadoFlujo = @idEstadoFlujoPreAprobado;
if (@idEstadoOrden = @idEstadoOrdenAnulado)
	SET @idEstadoFlujo = @idEstadoFlujoAnulado;
if (@idEstadoOrden = @idEstadoOrdenAprobado or @idEstadoOrden = @idEstadoOrdenEntregado or @idEstadoOrden = @idEstadoOrdenPagado or @idEstadoOrden = @idEstadoOrdenConErrores)
	SET @idEstadoFlujo = @idEstadoFlujoAprobado;

SELECT
	@comentarios = Comentarios
FROM
	FlujoAprobacion
WHERE
	IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden
	and IdTipoOrden = @idTipoOrden and IdEstadoFlujo = @idEstadoFlujo;

return @comentarios;

END
go

CREATE PROCEDURE spw_hth_consultarEstadoOrdenBancaria
/*************************************************************************
Nombre: spw_hth_consultarEstadoOrdenBancaria
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_consultarEstadoOrdenBancaria '0010', '2200073320', '2017', '20171229';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@esHorarioBanco bit = 0,
	@fechaActual datetime,
	@horaActual int = 0,
	@minutoActual int = 0,
	@horaInicioBanco int = 07,
	@minutoInicioBanco int = 00,
	@horaFinBanco int = 20,
	@minutoFinBanco int = 45;

SELECT @fechaActual = GETDATE();
SET @horaActual = DATEPART(HOUR, @fechaActual);
SET @minutoActual = DATEPART(MINUTE, @fechaActual);

IF (@horaActual >= @horaInicioBanco and @horaActual <= @horaFinBanco)
BEGIN
	SET @esHorarioBanco = 1;

	IF (@horaActual = @horaInicioBanco and @minutoActual <= @minutoInicioBanco)
	BEGIN
		SET @esHorarioBanco = 0;
	END
	IF (@horaActual = @horaFinBanco and @minutoActual >= @minutoFinBanco)
	BEGIN
		SET @esHorarioBanco = 0;
	END
END

SELECT
	IdEstadoOrden,
	@esHorarioBanco as esHorarioBanco
FROM
	OrdenBancaria
WHERE
	IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden and IdTipoOrden = @idTipoOrden;
	
END
go

CREATE PROCEDURE spw_hth_actualizarEstadoOrdenBancaria
/*************************************************************************
Nombre: spw_hth_actualizarEstadoOrdenBancaria
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_actualizarEstadoOrdenBancaria '000002', '000001', 'LI';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5),
@idUsuario nchar(6),
@idEstadoOrden nchar(2) OUTPUT,
@comentarios nvarchar(300)
)
AS
BEGIN TRY
SET NOCOUNT ON;

declare
	@codigo int = 0,
	@mensaje nvarchar(200) = '',
	@correlativo int = 0,
	@idFlujoAprobacion nchar(6) = '',
	@idEstadoFlujo nchar(3) = '',
	@usuarioCreacion nvarchar(20) = '',
	@usuarioPreAprobador nvarchar(20) = '',
	@idEstadoFlujoLiberado nchar(3) = '001',
	@idEstadoFlujoDeshecho nchar(3) = '002',
	@idEstadoFlujoPreAprobado nchar(3) = '003',
	@idEstadoFlujoAnulado nchar(3) = '004',
	@idEstadoFlujoAprobado nchar(3) = '005',
	@idEstadoFlujoEntregado nchar(3) = '006',
	@idEstadoOrdenLiberado nchar(2) = 'LI',
	@idEstadoOrdenDeshecho nchar(2) = 'DE',
	@idEstadoOrdenPreAprobado nchar(2) = 'PP',
	@idEstadoOrdenAnulado nchar(2) = 'AN',
	@idEstadoOrdenAprobado nchar(2) = 'AP',
	@idEstadoOrdenEntregado nchar(2) = 'ET';

IF (@idSociedad = '' or @idSap = '' or @anio = '' or @momentoOrden = '' or @idEstadoOrden = '' or @idUsuario = '')
BEGIN
	SET @codigo = -10;
	SET @mensaje = 'Parámetros no presentes en procedimiento almacenado, revisar invocación.';
END
ELSE
BEGIN
	BEGIN TRANSACTION

	IF (@idEstadoOrden = @idEstadoOrdenLiberado)
		SET @idEstadoOrden = @idEstadoOrdenPreAprobado;
	ELSE IF (@idEstadoOrden = @idEstadoOrdenPreAprobado)
		SET @idEstadoOrden = @idEstadoOrdenAprobado;

	UPDATE
		dbo.OrdenBancaria
	SET
		IdEstadoOrden = @idEstadoOrden
	WHERE
		IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden and IdTipoOrden = @idTipoOrden;
	
	IF (@idEstadoOrden = @idEstadoOrdenLiberado)
		SET @idEstadoFlujo = @idEstadoFlujoLiberado;
	ELSE IF (@idEstadoOrden = @idEstadoOrdenDeshecho)
		SET @idEstadoFlujo = @idEstadoFlujoDeshecho;
	ELSE IF (@idEstadoOrden = @idEstadoOrdenPreAprobado)
		SET @idEstadoFlujo = @idEstadoFlujoPreAprobado;
	ELSE IF (@idEstadoOrden = @idEstadoOrdenAnulado)
		SET @idEstadoFlujo = @idEstadoFlujoAnulado;
	ELSE IF (@idEstadoOrden = @idEstadoOrdenAprobado)
		SET @idEstadoFlujo = @idEstadoFlujoAprobado;
	ELSE IF (@idEstadoOrden = @idEstadoOrdenEntregado)
		SET @idEstadoFlujo = @idEstadoFlujoEntregado;

	SELECT @correlativo = ISNULL(MAX(IdFlujoAprobacion), 0) FROM dbo.FlujoAprobacion;
	SELECT @correlativo = @correlativo + 1;
	SELECT @idFlujoAprobacion = RIGHT(REPLICATE('0', 6) + CAST(@correlativo as nvarchar), 6);
	SELECT @usuarioCreacion = UserName FROM dbo.AspNetUsers WHERE IdUsuario = @idUsuario;
	
	INSERT INTO
		FlujoAprobacion
		(
		IdFlujoAprobacion,
		IdSociedad,
		IdSap,
		Anio,
		MomentoOrden,
		IdTipoOrden,
		IdUsuario,
		IdEstadoFlujo,
		Comentarios,
		UsuarioCreacion,
		FechaCreacion
		)
	VALUES
		(
		@idFlujoAprobacion,
		@idSociedad,
		@idSap,
		@anio,
		@momentoOrden,
		@idTipoOrden,
		@idUsuario,
		@idEstadoFlujo,
		@comentarios,
		@usuarioCreacion,
		GETDATE()
		);

	COMMIT TRANSACTION

	SET @codigo = 10;
	SET @mensaje = 'Se actualizó satisfactoriamente el estado de la orden bancaria.';
END

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION
print 'Error Código: ' + CAST(ERROR_NUMBER() as nvarchar)
print 'Error Línea: ' + CAST(ERROR_LINE() as nvarchar);
print 'Error Mensaje: ' + ERROR_MESSAGE();
SET @codigo = -15;
SET @mensaje = ERROR_MESSAGE(); 

END CATCH
SELECT @codigo as codigo, @mensaje as mensaje;
go

CREATE FUNCTION fn_hth_convertirCadenaHaciaTabla
/*************************************************************************
Nombre: fn_hth_convertirCadenaHaciaTabla
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
SELECT Id, Valor FROM dbo.fn_hth_convertirCadenaHaciaTabla ('0010,2200073320,2017,20171229', ',');

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@cadena nvarchar(MAX),
@delimitador nvarchar(255)
)
RETURNS TABLE

WITH SCHEMABINDING
AS
RETURN

WITH	E1(N)		AS ( SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1
                         UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 
                         UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1),
		E2(N)       AS (SELECT 1 FROM E1 a, E1 b),
		E4(N)       AS (SELECT 1 FROM E2 a, E2 b),
		E42(N)      AS (SELECT 1 FROM E4 a, E2 b),
		cteTally(N) AS (SELECT 0 UNION ALL SELECT TOP (DATALENGTH(ISNULL(@cadena, 1))) 
                         ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) FROM E42),
		cteStart(N1)AS (SELECT t.N+1 FROM cteTally t
                         WHERE (SUBSTRING(@cadena, t.N, 1) = @delimitador OR t.N = 0))
SELECT
	ROW_NUMBER() OVER (ORDER BY s.N1) as Id,
	Valor = SUBSTRING(@cadena, s.N1, ISNULL(NULLIF(CHARINDEX(@delimitador, @cadena, s.N1), 0)-s.N1,8000))
FROM
	cteStart s;
go

CREATE PROCEDURE spw_hth_consultarEstadoMasivoOrdenesBancarias
/*************************************************************************
Nombre: spw_hth_consultarEstadoMasivoOrdenesBancarias
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_consultarEstadoMasivoOrdenesBancarias '0010|00004R|2018|20181224|0010B000144.txt|D:\\HTH\\CORRECTO\\0010B000144.txt&0010|00003R|2018|20181224|0010B000141.txt|D:\\HTH\\CORRECTO\\0010B000141.txt&0020|2794|2017|20171201|0020P000140.txt|D:\\HTH\\CORRECTO\\0020P000140.txt';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@cadena nvarchar(4000)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@esHorarioBanco bit = 0,
	@fechaActual datetime,
	@horaActual int = 0,
	@minutoActual int = 0,
	@horaInicioBanco int = 07,
	@minutoInicioBanco int = 00,
	@horaFinBanco int = 20,
	@minutoFinBanco int = 45,
	@delimitadorAmperson nchar(1) = '&',
	@delimitadorPipe nchar(1) = '|',
	@contador int = 1,
	@contadorTotal int = 0,
	@parametros nvarchar(255) = '',
	@idSociedad nchar(4) = '',
	@idSap nvarchar(10) = '',
	@anio nchar(4) = '',
	@momentoOrden nchar(8) = '',
	@idTipoOrden nvarchar(5) = '',
	@idEstadoOrdenLiberado nchar(2) = 'LI',
	@idEstadoOrdenPreAprobado nchar(2) = 'PP',
	@idEstadoOrden nchar(2) = '',
	@esEstadoLiberado bit = 0,
	@esEstadoPreAprobado bit = 0,
	@INDICE_1 int = 1,
	@INDICE_2 int = 2,
	@INDICE_3 int = 3,
	@INDICE_4 int = 4,
	@INDICE_5 int = 5;

declare @tablaPadre as table
(
Id int,
Valor nvarchar(255)
);
declare @tablaHijo as table
(
Id int,
Valor nvarchar(255)
);

SELECT @fechaActual = GETDATE();
SET @horaActual = DATEPART(HOUR, @fechaActual);
SET @minutoActual = DATEPART(MINUTE, @fechaActual);

IF (@horaActual >= @horaInicioBanco and @horaActual <= @horaFinBanco)
BEGIN
	SET @esHorarioBanco = 1;

	IF (@horaActual = @horaInicioBanco and @minutoActual <= @minutoInicioBanco)
	BEGIN
		SET @esHorarioBanco = 0;
	END
	IF (@horaActual = @horaFinBanco and @minutoActual >= @minutoFinBanco)
	BEGIN
		SET @esHorarioBanco = 0;
	END
END

INSERT INTO @tablaPadre
SELECT Id, Valor FROM dbo.fn_hth_convertirCadenaHaciaTabla (@cadena, @delimitadorAmperson);

SELECT @contadorTotal = COUNT(Id) FROM @tablaPadre;

WHILE (@contador <= @contadorTotal)
BEGIN
	SELECT @parametros = Valor FROM @tablaPadre WHERE Id = @contador;
	
	INSERT INTO @tablaHijo
	SELECT Id, Valor FROM dbo.fn_hth_convertirCadenaHaciaTabla (@parametros, @delimitadorPipe);

	SELECT @idSociedad = Valor FROM @tablaHijo WHERE Id = @INDICE_1;
	SELECT @idSap = Valor FROM @tablaHijo WHERE Id = @INDICE_2;
	SELECT @anio = Valor FROM @tablaHijo WHERE Id = @INDICE_3;
	SELECT @momentoOrden = Valor FROM @tablaHijo WHERE Id = @INDICE_4;
	SELECT @idTipoOrden = Valor FROM @tablaHijo WHERE Id = @INDICE_5;
	
	SELECT
		@idEstadoOrden = IdEstadoOrden
	FROM
		OrdenBancaria
	WHERE
		IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden and IdTipoOrden = @idTipoOrden;

	IF (@idEstadoOrden = @idEstadoOrdenLiberado)
	BEGIN
		SET @esEstadoLiberado = 1;
	END
	ELSE IF (@idEstadoOrden = @idEstadoOrdenPreAprobado)
	BEGIN
		SET @esEstadoPreAprobado = 1;
	END

	DELETE FROM @tablaHijo;
	SELECT @parametros = '', @idSociedad = '', @idSap = '', @anio = '', @momentoOrden = '', @idEstadoOrden = '';
	SET @contador = @contador + 1;
END

SELECT
	@esEstadoLiberado as EsEstadoLiberado,
	@esEstadoPreAprobado as EsEstadoPreAprobado,
	@esHorarioBanco as EsHorarioBanco;
	
END
go

CREATE PROCEDURE spw_hth_actualizarEstadoMasivoOrdenBancaria
/*************************************************************************
Nombre: spw_hth_actualizarEstadoMasivoOrdenBancaria
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_actualizarEstadoMasivoOrdenBancaria '0010', '00004R', '2018', '20181224', '000001', 'LI';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5),
@idUsuario nchar(6),
@idEstadoOrden nchar(2) OUTPUT,
@comentarios nvarchar(300)
)
AS
SET NOCOUNT ON;
BEGIN TRY

declare
	@codigo int = 0,
	@mensaje nvarchar(200) = '',
	@correlativo int = 0,
	@idFlujoAprobacion nchar(6) = '',
	@usuarioCreacion nvarchar(20) = '',
	@idEstadoFlujo nchar(3) = '',
	@idEstadoFlujoPreAprobado nchar(3) = '003',
	@idEstadoFlujoAprobado nchar(3) = '005',
	@idEstadoFlujoEntregado nchar(3) = '006',
	@idEstadoOrdenLiberado nchar(2) = 'LI',
	@idEstadoOrdenPreAprobado nchar(2) = 'PP',
	@idEstadoOrdenAprobado nchar(2) = 'AP',
	@idEstadoOrdenEntregado nchar(2) = 'ET';

IF (@idSociedad = '' or @idSap = '' or @anio = '' or @momentoOrden = '' or @idTipoOrden = '' or @idUsuario = '' or @idEstadoOrden = '')
BEGIN
	SET @codigo = -10;
	SET @mensaje = 'Parámetros no presentes en procedimiento almacenado, revisar invocación.';
END
ELSE
BEGIN
	BEGIN TRANSACTION

	IF (@idEstadoOrden = @idEstadoOrdenLiberado)
		SET @idEstadoOrden = @idEstadoOrdenPreAprobado;
	ELSE IF (@idEstadoOrden = @idEstadoOrdenPreAprobado)
		SET @idEstadoOrden = @idEstadoOrdenAprobado;

	UPDATE
		dbo.OrdenBancaria
	SET
		IdEstadoOrden = @idEstadoOrden
	WHERE
		IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden and IdTipoOrden = @idTipoOrden;

	IF (@idEstadoOrden = @idEstadoOrdenPreAprobado)
		SET @idEstadoFlujo = @idEstadoFlujoPreAprobado;
	ELSE IF (@idEstadoOrden = @idEstadoOrdenAprobado)
		SET @idEstadoFlujo = @idEstadoFlujoAprobado;
	ELSE IF (@idEstadoOrden = @idEstadoOrdenEntregado)
		SET @idEstadoFlujo = @idEstadoFlujoEntregado;

	SELECT @correlativo = ISNULL(MAX(IdFlujoAprobacion), 0) FROM dbo.FlujoAprobacion;
	SELECT @correlativo = @correlativo + 1;
	SELECT @idFlujoAprobacion = RIGHT(REPLICATE('0', 6) + CAST(@correlativo as nvarchar), 6);
	SELECT @usuarioCreacion = UserName FROM dbo.AspNetUsers WHERE IdUsuario = @idUsuario;
	
	INSERT INTO
		FlujoAprobacion
		(
		IdFlujoAprobacion,
		IdSociedad,
		IdSap,
		Anio,
		MomentoOrden,
		IdTipoOrden,
		IdUsuario,
		IdEstadoFlujo,
		Comentarios,
		UsuarioCreacion,
		FechaCreacion
		)
	VALUES
		(
		@idFlujoAprobacion,
		@idSociedad,
		@idSap,
		@anio,
		@momentoOrden,
		@idTipoOrden,
		@idUsuario,
		@idEstadoFlujo,
		@comentarios,
		@usuarioCreacion,
		GETDATE()
		);

	COMMIT TRANSACTION

	SET @codigo = 10;
	SET @mensaje = 'Se actualizó satisfactoriamente el estado masivo de la orden bancaria.';
END

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION
print 'Error Código: ' + CAST(ERROR_NUMBER() as nvarchar)
print 'Error Línea: ' + CAST(ERROR_LINE() as nvarchar);
print 'Error Mensaje: ' + ERROR_MESSAGE();
SET @codigo = -15;
SET @mensaje = ERROR_MESSAGE(); 

END CATCH
SELECT @codigo as codigo, @mensaje as mensaje;
go

CREATE PROCEDURE spw_hth_listarFlujoAprobacion
/*************************************************************************
Nombre: spw_hth_listarFlujoAprobacion
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarFlujoAprobacion '0010', '2200000054', '2018', '20180101';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@idTipoOrden nvarchar(5)
)
AS
BEGIN
SET NOCOUNT ON;

SELECT
	fa.IdFlujoAprobacion as IdFlujoAprobacion,
	ef.EstadoFlujo as EstadoFlujo,
	ef.Abreviacion as EstadoFlujoCorto,
	fa.Comentarios,
	au.UserName,
	fa.FechaCreacion
FROM
	FlujoAprobacion fa
	join dbo.AspNetUsers au on fa.Idusuario = au.IdUsuario
	join EstadoFlujo ef on fa.IdEstadoFlujo = ef.IdEstadoFlujo and ef.Estado = 1
WHERE
	fa.IdSociedad = @idSociedad and fa.IdSap = @idSap and fa.Anio = @anio and fa.MomentoOrden = @momentoOrden and fa.IdTipoOrden = @idTipoOrden
ORDER BY
	fa.FechaCreacion ASC;
	
END
go

CREATE PROCEDURE spw_hth_enviarRespuestaProcesoHaciaSap
/*************************************************************************
Nombre: spw_hth_enviarRespuestaProcesoHaciaSap
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_enviarRespuestaProcesoHaciaSap '0010', '2200000054', '2018', '20180101';
exec spw_hth_enviarRespuestaProcesoHaciaSap '0010', '1304', '2017', '20170303';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idSociedad nchar(4),
@idSap nvarchar(10),
@anio nchar(4),
@momentoOrden nchar(8),
@tipoOrden nchar(3)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idTipoOrdenTransferencia nvarchar(5) = '06',
	@idTipoOrdenTransferenciaBcr nvarchar(5) = '07',
	@idTipoOrdenProveedor nvarchar(5) = '01',
	@idTipoOrdenCamaraComercio nvarchar(5) = '03',
	@idTipoOrden nvarchar(5) = '',
	@tipoOrdenTransferencia nchar(3) = 'TRA',
	@tipoOrdenTransferenciaBcr nchar(3) = 'BCR',
	@tipoOrdenProveedor nchar(3) = 'PRO',
	@tipoOrdenCamaraComercio nchar(3) = 'CCE',
	@idEstadoOrden nchar(2) = '',
	@idEstadoFlujo nchar(3) = '',
	@idEstadoOrdenLiberado nchar(2) = 'LI',
	@idEstadoOrdenDeshecho nchar(2) = 'DE',
	@idEstadoOrdenPreAprobado nchar(2) = 'PP',
	@idEstadoOrdenAnulado nchar(2) = 'AN',
	@idEstadoOrdenAprobado nchar(2) = 'AP',
	@idEstadoOrdenEntregado nchar(2) = 'ET',
	@idEstadoOrdenPagado nchar(2) = 'PA',
	@idEstadoOrdenConErrores nchar(2) = 'CE',
	@idEstadoFlujoLiberado nchar(3) = '001',
	@idEstadoFlujoDeshecho nchar(3) = '002',
	@idEstadoFlujoPreAprobado nchar(3) = '003',
	@idEstadoFlujoAnulado nchar(3) = '004',
	@idEstadoFlujoAprobado nchar(3) = '005',
	@idEstadoFlujoEntregado nchar(3) = '006',
	@idEstadoFlujoPagado nchar(3) = '007',
	@idEstadoFlujoConErrores nchar(3) = '008';

SELECT @idTipoOrden = CASE @tipoOrden WHEN @tipoOrdenTransferencia THEN @idTipoOrdenTransferencia WHEN @tipoOrdenTransferenciaBcr THEN @idTipoOrdenTransferenciaBcr WHEN @tipoOrdenProveedor THEN @idTipoOrdenProveedor WHEN @tipoOrdenCamaraComercio THEN @idTipoOrdenCamaraComercio ELSE '' END;
SELECT @idEstadoOrden = idEstadoOrden FROM OrdenBancaria WHERE IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden and IdTipoOrden = @idTipoOrden;

if (@idEstadoOrden = @idEstadoOrdenLiberado)
	SET @idEstadoFlujo = @idEstadoFlujoLiberado;
if (@idEstadoOrden = @idEstadoOrdenDeshecho)
	SET @idEstadoFlujo = @idEstadoFlujoDeshecho;
if (@idEstadoOrden = @idEstadoOrdenPreAprobado)
	SET @idEstadoFlujo = @idEstadoFlujoPreAprobado;
if (@idEstadoOrden = @idEstadoOrdenAnulado)
	SET @idEstadoFlujo = @idEstadoFlujoAnulado;
if (@idEstadoOrden = @idEstadoOrdenAprobado)
	SET @idEstadoFlujo = @idEstadoFlujoAprobado;
if (@idEstadoOrden = @idEstadoOrdenEntregado)
	SET @idEstadoFlujo = @idEstadoFlujoEntregado;
if (@idEstadoOrden = @idEstadoOrdenPagado)
	SET @idEstadoFlujo = @idEstadoFlujoPagado;
if (@idEstadoOrden = @idEstadoOrdenConErrores)
	SET @idEstadoFlujo = @idEstadoFlujoConErrores;

SELECT
	ob.IdEstadoOrden,
	eo.EstadoOrden,
	fa.UsuarioCreacion,
	CONVERT(nvarchar(10), ISNULL(fa.FechaCreacion, null), 103),
	CONVERT(nvarchar(8), ISNULL(fa.FechaCreacion, null), 108)
FROM
	OrdenBancaria ob
	join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
	join 
	(
	SELECT IdSociedad, IdSap, Anio, MomentoOrden, IdTipoOrden, UsuarioCreacion, FechaCreacion
	FROM FlujoAprobacion
	WHERE IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden and IdTipoOrden = @idTipoOrden and IdEstadoFlujo = @idEstadoFlujo
	) fa on ob.IdSociedad = fa.IdSociedad and ob.IdSap = fa.IdSap and ob.Anio = fa.Anio and ob.MomentoOrden = fa.MomentoOrden and ob.IdTipoOrden = fa.IdTipoOrden
WHERE
	ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden and ob.IdTipoOrden = @idTipoOrden;

SELECT
	RTRIM(LTRIM(Beneficiario)),
	SUBSTRING(RTRIM(LTRIM(Referencia1)), 1, 10),
	CASE ob.IdTipoOrden WHEN @idTipoOrdenTransferencia THEN obd.MontoAbono WHEN @idTipoOrdenTransferenciaBcr then obd.MontoCargo WHEN @idTipoOrdenProveedor THEN obd.MontoAbono END,
	RTRIM(LTRIM(IdRespuesta)),
	RTRIM(LTRIM(Respuesta))
FROM
	OrdenBancaria ob
	join OrdenBancariaDetalle obd on ob.IdSociedad = obd.IdSociedad and ob.IdSap = obd.IdSap and ob.Anio = obd.Anio and ob.MomentoOrden = obd.MomentoOrden and ob.IdTipoOrden = obd.IdTipoOrden
WHERE
	ob.IdSociedad = @idSociedad and ob.IdSap = @idSap and ob.Anio = @anio and ob.MomentoOrden = @momentoOrden and ob.IdTipoOrden = @idTipoOrden
GROUP BY
	obd.Beneficiario, obd.Referencia1, obd.MontoAbono, obd.IdRespuesta, obd.Respuesta, ob.IdTipoOrden, obd.MontoAbono,
	obd.MontoCargo, ob.IdEstadoOrden
HAVING
	ob.IdEstadoOrden in (@idEstadoOrdenPagado, @idEstadoOrdenConErrores)
ORDER BY
	obd.Referencia1 ASC;

END
go

CREATE PROCEDURE sps_hth_procesarRespuesta
/*************************************************************************
Nombre: sps_hth_procesarRespuesta
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec sps_hth_procesarRespuesta '0010P000020.txt',
N'<?xml version="1.0" encoding="utf-16"?>
<tramas>
	<trama><id>1</id><cadena>0622000000541018 CS_VSULCA       00428424000001        201801010000962022190630YURA S.A.                                                   429335500001        0000009620221000000000000000520312372895                                                            049DIGITO DE CHEQUEO NO VALIDO EN NETOS              0009500087                 </cadena></trama>
	<trama><id>2</id><cadena>0622000000541018 CS_VSULCA       00428424000001        201801010000962022190630YURA S.A.                                                   429335500001        0000009620221000000000000000520312372895                                                            049DIGITO DE CHEQUEO NO VALIDO EN NETOS              0012400087                 </cadena></trama>
	<trama><id>3</id><cadena>0622000000541018 CS_VSULCA       00428424000001        201801010000962022190630YURA S.A.                                                   429335500001        0000009620221000000000000000520312372895                                                            049DIGITO DE CHEQUEO NO VALIDO EN NETOS              0014700087                 </cadena></trama>
</tramas>';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@nombreArchivo nvarchar(100),
@tramaRespuesta xml
)
AS
BEGIN TRY

declare
	@codigo int = 0,
	@mensaje nvarchar(200) = '',
	@contadorProcesado int = 0,
	@contador int = 1,
	@contadorTotal int = 0,
	@idUsuario nchar(6) = '',
	@idFlujoAprobacion nchar(6) = '',
	@correlativoFlujoAprobacion int = 0,
	@idEstadoOrden nchar(2) = '',
	@idEstadoOrdenPagado nchar(2) = 'PA',
	@idEstadoOrdenConErrores nchar(2) = 'CE',
	@idEstadoFlujo nchar(3) = '',
	@idEstadoFlujoAprobado nchar(3) = '005',
	@idEstadoFlujoPagado nchar(3) = '007',
	@idEstadoFlujoConErrores nchar(3) = '008',
	@cadena nvarchar(500) = '',
	@idSociedad nchar(4) = '',
	@idSap nvarchar(10) = '',
	@anio nchar(4) = '',
	@momentoOrden nchar(8) = '',
	@idBanco nchar(5) = '',
	@idTipoOrden nvarchar(5) = '',
	@tipoOrden nvarchar(20) = '',
	@idTipoEnvio nchar(3) = '001',
	@idTipoRespuesta nchar(3) = '002',
	@procesadoOK nchar(3) = '000',
	@esTerminado bit = 1,
	@usuarioHostToHost nvarchar(20) = 'Servicio Windows',
	@desdeReferencia1 int = 0,
	@longitudReferencia1 int = 0,
	@desdeIdRespuesta int = 0,
	@longitudIdRespuesta int = 0,
	@desdeRespuesta int = 0,
	@longitudRespuesta int = 0,
	@desdeNroOrden int = 0,
	@longitudNroOrden int = 0,
	@desdeNroConvenio int = 0,
	@longitudNroConvenio int = 0,
	@desdeHoraProceso int = 0,
	@longitudHoraProceso int = 0,
	@desdeTrama int = 0,
	@longitudTrama int = 0,
	@REFERENCIA_1 nvarchar(50) = 'Referencia1',
	@CODIGO_RESPUESTA nvarchar(50) = 'CodigoRespuesta',
	@DESCRIPCION_RESPUESTA nvarchar(50) = 'DescripcionRespuesta',
	@NUMERO_ORDEN nvarchar(50) = 'NumeroOrden',
	@NUMERO_CONVENIO nvarchar(50) = 'NumeroConvenio',
	@HORA_PROCESO nvarchar(50) = 'HoraProceso',
	@referencia1 nvarchar(50) = '',
	@idRespuesta nvarchar(10) = '',
	@respuesta nvarchar(200) = '',
	@nroOrden nvarchar(50) = '',
	@nroConvenio nvarchar(50) = '',
	@horaProceso nvarchar(10) = '';

declare @trama as table
(
id int,
cadena nvarchar(500)
);

BEGIN TRANSACTION

INSERT INTO @trama
SELECT
	trama.col.value('id[1]', 'int') id,
	trama.col.value('cadena[1]', 'nvarchar(500)') cadena
FROM
	@tramaRespuesta.nodes('/tramas/trama') trama(col);

SELECT
	@idSociedad = ob.IdSociedad,
	@idSap = ob.IdSap,
	@anio = ob.Anio,
	@momentoOrden = ob.MomentoOrden,
	@idBanco = ob.IdBanco,
	@idTipoOrden = ob.IdTipoOrden,
	@tipoOrden = too.Abreviacion
FROM
	OrdenBancaria ob
	join TipoOrden too on ob.IdTipoOrden = too.IdTipoOrden and too.Estado = 1
WHERE
	NombreArchivo = @nombreArchivo;

SELECT @idUsuario = IdUsuario FROM FlujoAprobacion WHERE IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden and IdTipoOrden = @idTipoOrden and IdEstadoFlujo = @idEstadoFlujoAprobado;
SELECT @contadorTotal = COUNT(cadena) FROM @trama;
SELECT @desdeReferencia1 = Desde, @longitudReferencia1 = Longitud FROM TramaEstructura WHERE IdBanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoEnvio and Estructura = @REFERENCIA_1;
SELECT @desdeIdRespuesta = Desde, @longitudIdRespuesta = Longitud FROM TramaEstructura WHERE Idbanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoRespuesta and Estructura = @CODIGO_RESPUESTA;
SELECT @desdeRespuesta = Desde, @longitudRespuesta = Longitud FROM TramaEstructura WHERE Idbanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoRespuesta and Estructura = @DESCRIPCION_RESPUESTA;
SELECT @desdeNroOrden = Desde, @longitudNroOrden = Longitud FROM TramaEstructura WHERE Idbanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoRespuesta and Estructura = @NUMERO_ORDEN;
SELECT @desdeNroConvenio = Desde, @longitudNroConvenio = Longitud FROM TramaEstructura WHERE Idbanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoRespuesta and Estructura = @NUMERO_CONVENIO;
SELECT @desdeHoraProceso = Desde, @longitudHoraProceso = Longitud FROM TramaEstructura WHERE Idbanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoRespuesta and Estructura = @HORA_PROCESO;
SELECT @desdeTrama = Desde FROM TramaEstructura WHERE Idbanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoRespuesta and Estructura = @CODIGO_RESPUESTA;
SELECT @longitudTrama = Hasta FROM TramaEstructura WHERE Idbanco = @idBanco and IdTipoOrden = @idTipoOrden and IdTipo = @idTipoRespuesta and Estructura = @HORA_PROCESO;
SELECT @longitudTrama = @longitudTrama - @desdeTrama + 1;

WHILE (@contador <= @contadorTotal)
BEGIN
	SELECT @cadena = cadena FROM @trama WHERE id = @contador;

	SELECT @referencia1 = SUBSTRING(@cadena, @desdeReferencia1, @longitudReferencia1);
	SELECT @idRespuesta = SUBSTRING(@cadena, @desdeIdRespuesta, @longitudIdRespuesta);
	SELECT @respuesta = SUBSTRING(@cadena, @desdeRespuesta, @longitudRespuesta);
	SELECT @nroOrden = SUBSTRING(@cadena, @desdeNroOrden, @longitudNroOrden);
	SELECT @nroConvenio = SUBSTRING(@cadena, @desdeNroConvenio, @longitudNroConvenio);
	SELECT @horaProceso = SUBSTRING(@cadena, @desdeHoraProceso, @longitudHoraProceso);

	UPDATE
		OrdenBancariaDetalle
	SET
		IdRespuesta = @idRespuesta,
		Respuesta = @respuesta,
		NroOrden = @nroOrden,
		NroConvenio = @nroConvenio,
		UsuarioEdicion = @usuarioHostToHost,
		FechaEdicion = GETDATE()
	WHERE
		IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden and IdTipoOrden = @idTipoOrden and IdBanco = @idBanco and Referencia1 = @referencia1;

	UPDATE
		TramaDetalle
	SET
		Respuesta = SUBSTRING(@cadena, @desdeTrama, @longitudTrama),
		UsuarioEdicion = @usuarioHostToHost,
		FechaEdicion = GETDATE()
	WHERE
		IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden and IdTipoOrden = @idTipoOrden and Referencia2 = @referencia1;

	IF (@idRespuesta = @procesadoOK)
	BEGIN
		SET @contadorProcesado = @contadorProcesado + 1;
	END

	SELECT @cadena = '', @idRespuesta = '', @respuesta = '', @nroOrden = '', @nroConvenio = '';
	SET @contador = @contador + 1;
END

IF (@contadorProcesado = @contadorTotal)
BEGIN
	SET @idEstadoOrden = @idEstadoOrdenPagado;
	SET @idEstadoFlujo = @idEstadoFlujoPagado;
END
ELSE
BEGIN
	SET @idEstadoOrden = @idEstadoOrdenConErrores;
	SET @idEstadoFlujo = @idEstadoFlujoConErrores;
END

UPDATE
	OrdenBancaria
SET
	IdEstadoOrden = @idEstadoOrden,
	UsuarioEdicion = @usuarioHostToHost,
	FechaEdicion = GETDATE()
WHERE
	IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden and IdTipoOrden = @idTipoOrden and IdBanco = @idBanco and NombreArchivo = @nombreArchivo;

UPDATE
	Trama
SET
	Terminado = @esTerminado
WHERE
	IdSociedad = @idSociedad and IdSap = @idSap and Anio = @anio and MomentoOrden = @momentoOrden and IdTipoOrden = @idTipoOrden and IdBanco = @idBanco and NombreArchivo = @nombreArchivo;

SELECT @correlativoFlujoAprobacion = ISNULL(MAX(IdFlujoAprobacion), 0) FROM FlujoAprobacion;
SELECT @correlativoFlujoAprobacion = @correlativoFlujoAprobacion + 1;
SET @idFlujoAprobacion = RIGHT(REPLICATE('0', 6) + CAST(@correlativoFlujoAprobacion as nvarchar), 6);

INSERT INTO
	FlujoAprobacion
	(
	IdFlujoAprobacion,
	IdSociedad,
	IdSap,
	Anio,
	MomentoOrden,
	IdTipoOrden,
	IdUsuario,
	IdEstadoFlujo,
	Comentarios,
	UsuarioCreacion,
	FechaCreacion
	)
VALUES
	(
	@idFlujoAprobacion,
	@idSociedad,
	@idSap,
	@anio,
	@momentoOrden,
	@idTipoOrden,
	@idUsuario,
	@idEstadoFlujo,
	null,
	@usuarioHostToHost,
	GETDATE()
	);

COMMIT TRANSACTION

SELECT @codigo = 10, @mensaje = 'Se procesó satisfactoriamente la respuesta del banco.';
SELECT
	@codigo as codigo,
	@mensaje as mensaje,
	@idSociedad,
	@idSap,
	@anio,
	@momentoOrden,
	@idEstadoOrden,
	@tipoOrden,
	@usuarioHostToHost;

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION
print 'Error Código: ' + CAST(ERROR_NUMBER() as nvarchar)
print 'Error Línea: ' + CAST(ERROR_LINE() as nvarchar);
print 'Error Mensaje: ' + ERROR_MESSAGE();
SET @codigo = -15;
SET @mensaje = ERROR_MESSAGE(); 

END CATCH
go

CREATE PROCEDURE spw_hth_listarFiltrosOrdenesBancariasLiberadas
/*************************************************************************
Nombre: spw_hth_listarFiltrosOrdenesBancariasLiberadas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarFiltrosOrdenesBancariasLiberadas '000013';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idEstadoOrdenLiberado nchar(2) = 'LI';

SELECT DISTINCT
	ob.IdSociedad,
	s.Abreviacion as SociedadCorto
FROM
	OrdenBancaria ob
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenLiberado)
	and ob.IdUsuario = @idUsuario
ORDER BY
	s.Abreviacion ASC;


SELECT DISTINCT
	ob.IdBanco,
	b.Abreviacion as BancoCorto
FROM
	OrdenBancaria ob
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenLiberado)
	and ob.IdUsuario = @idUsuario
ORDER BY
	b.Abreviacion ASC;


SELECT DISTINCT
	ob.IdTipoOrden,
	o.Abreviacion as TipoOrdenCorto
FROM
	OrdenBancaria ob
	join TipoOrden o on ob.IdTipoOrden = o.IdTipoOrden and o.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenLiberado)
	and ob.IdUsuario = @idUsuario
ORDER BY
	o.Abreviacion ASC;

END
go

CREATE PROCEDURE spw_hth_listarFiltrosOrdenesBancariasDesechas
/*************************************************************************
Nombre: spw_hth_listarFiltrosOrdenesBancariasDesechas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarFiltrosOrdenesBancariasDesechas '000013';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idEstadoOrdenDesecho nchar(2) = 'DE',
	@idEstadoOrdenAnulado nchar(2) = 'AN';

SELECT DISTINCT
	ob.IdSociedad,
	s.Abreviacion as SociedadCorto
FROM
	OrdenBancaria ob
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenDesecho, @idEstadoOrdenAnulado)
	and ob.IdUsuario = @idUsuario
ORDER BY
	s.Abreviacion ASC;


SELECT DISTINCT
	ob.IdBanco,
	b.Abreviacion as BancoCorto
FROM
	OrdenBancaria ob
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenDesecho, @idEstadoOrdenAnulado)
	and ob.IdUsuario = @idUsuario
ORDER BY
	b.Abreviacion ASC;


SELECT DISTINCT
	ob.IdTipoOrden,
	o.Abreviacion as TipoOrdenCorto
FROM
	OrdenBancaria ob
	join TipoOrden o on ob.IdTipoOrden = o.IdTipoOrden and o.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenDesecho, @idEstadoOrdenAnulado)
	and ob.IdUsuario = @idUsuario
ORDER BY
	o.Abreviacion ASC;

END
go

CREATE PROCEDURE spw_hth_listarFiltrosOrdenesBancariasPorAprobar
/*************************************************************************
Nombre: spw_hth_listarFiltrosOrdenesBancariasPorAprobar
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarFiltrosOrdenesBancariasPorAprobar '000002';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@usuario nvarchar(20) = '',
	@idEstadoOrdenLiberado nchar(2) = 'LI',
	@idEstadoOrdenPreAprobado nchar(2) = 'PP';

SELECT @usuario = UserName FROM dbo.AspNetUsers WHERE IdUsuario = @IdUsuario;

SELECT DISTINCT
	ob.IdSociedad,
	s.Abreviacion as SociedadCorto
FROM
	OrdenBancaria ob
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenLiberado, @idEstadoOrdenPreAprobado)
GROUP BY
	ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, s.Abreviacion
HAVING
	dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) <> @usuario
ORDER BY
	s.Abreviacion ASC;


SELECT DISTINCT
	ob.IdBanco,
	b.Abreviacion as BancoCorto
FROM
	OrdenBancaria ob
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenLiberado, @idEstadoOrdenPreAprobado)
GROUP BY
	ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdBanco, b.Abreviacion
HAVING
	dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) <> @usuario
ORDER BY
	b.Abreviacion ASC;


SELECT DISTINCT
	ob.IdTipoOrden,
	o.Abreviacion as TipoOrdenCorto
FROM
	OrdenBancaria ob
	join TipoOrden o on ob.IdTipoOrden = o.IdTipoOrden and o.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenLiberado, @idEstadoOrdenPreAprobado)
GROUP BY
	ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, o.Abreviacion
HAVING
	dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) <> @usuario
ORDER BY
	o.Abreviacion ASC;


SELECT DISTINCT
	ob.IdEstadoOrden,
	eo.EstadoOrden as EstadoOrden
FROM
	OrdenBancaria ob
	join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenLiberado, @idEstadoOrdenPreAprobado)
GROUP BY
	ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdEstadoOrden, eo.EstadoOrden
HAVING
	dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) <> @usuario
ORDER BY
	eo.EstadoOrden ASC;

END
go

CREATE PROCEDURE spw_hth_listarFiltrosOrdenesBancariasAprobadas
/*************************************************************************
Nombre: spw_hth_listarFiltrosOrdenesBancariasAprobadas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarFiltrosOrdenesBancariasAprobadas;

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@usuario nvarchar(20) = '',
	@idEstadoOrdenPreAprobado nchar(2) = 'PP',
	@idEstadoOrdenAprobado nchar(2) = 'AP',
	@idEstadoOrdenEntregado nchar(2) = 'ET',
	@idEstadoOrdenPagado nchar(2) = 'PA',
	@idEstadoOrdenConErrores nchar(2) = 'CE';

SELECT @usuario = UserName FROM dbo.AspNetUsers WHERE IdUsuario = @IdUsuario;

SELECT DISTINCT
	ob.IdSociedad,
	s.Abreviacion as SociedadCorto
FROM
	OrdenBancaria ob
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenPreAprobado, @idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
GROUP BY
	ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, s.Abreviacion
HAVING
	(dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario or dbo.fn_hth_obtenerAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario)
ORDER BY
	s.Abreviacion ASC;


SELECT DISTINCT
	ob.IdBanco,
	b.Abreviacion as BancoCorto
FROM
	OrdenBancaria ob
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenPreAprobado, @idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
GROUP BY
	ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdBanco, b.Abreviacion
HAVING
	(dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario or dbo.fn_hth_obtenerAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario)
ORDER BY
	b.Abreviacion ASC;


SELECT DISTINCT
	ob.IdTipoOrden,
	o.Abreviacion as TipoOrdenCorto
FROM
	OrdenBancaria ob
	join TipoOrden o on ob.IdTipoOrden = o.IdTipoOrden and o.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenPreAprobado, @idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
GROUP BY
	ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, o.Abreviacion
HAVING
	(dbo.fn_hth_obtenerPreAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario or dbo.fn_hth_obtenerAprobador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario)
ORDER BY
	o.Abreviacion ASC;

END
go

CREATE PROCEDURE spw_hth_listarFiltrosOrdenesBancariasAnuladas
/*************************************************************************
Nombre: spw_hth_listarFiltrosOrdenesBancariasAnuladas
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarFiltrosOrdenesBancariasAnuladas '000002';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@usuario nvarchar(20) = '',
	@idEstadoOrdenAnulado nchar(2) = 'AN';

SELECT @usuario = UserName FROM dbo.AspNetUsers WHERE IdUsuario = @IdUsuario;

SELECT DISTINCT
	ob.IdSociedad,
	s.Abreviacion as SociedadCorto
FROM
	OrdenBancaria ob
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenAnulado)
GROUP BY
	ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, s.Abreviacion
HAVING
	dbo.fn_hth_obtenerAnulador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario
ORDER BY
	s.Abreviacion ASC;


SELECT DISTINCT
	ob.IdBanco,
	b.Abreviacion as BancoCorto
FROM
	OrdenBancaria ob
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
WHERE
	ob.IdUsuario = @idUsuario and ob.IdEstadoOrden in (@idEstadoOrdenAnulado)
GROUP BY
	ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, ob.IdBanco, b.Abreviacion
HAVING
	dbo.fn_hth_obtenerAnulador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario
ORDER BY
	b.Abreviacion ASC;


SELECT DISTINCT
	ob.IdTipoOrden,
	o.Abreviacion as TipoOrdenCorto
FROM
	OrdenBancaria ob
	join TipoOrden o on ob.IdTipoOrden = o.IdTipoOrden and o.Estado = 1
WHERE
	ob.IdUsuario = @idUsuario and ob.IdEstadoOrden in (@idEstadoOrdenAnulado)
GROUP BY
	ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden, o.Abreviacion
HAVING
	dbo.fn_hth_obtenerAnulador(ob.IdSociedad, ob.IdSap, ob.Anio, ob.MomentoOrden, ob.IdTipoOrden) = @usuario
ORDER BY
	o.Abreviacion ASC;

END
go

CREATE PROCEDURE spw_hth_listarFiltrosOrdenesBancariasAprobadasTesoreria
/*************************************************************************
Nombre: spw_hth_listarFiltrosOrdenesBancariasAprobadasTesoreria
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarFiltrosOrdenesBancariasAprobadasTesoreria '000013';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6)
)
AS
BEGIN
SET NOCOUNT ON;

declare
	@idEstadoOrdenPreAprobado nchar(2) = 'PP',
	@idEstadoOrdenAprobado nchar(2) = 'AP',
	@idEstadoOrdenEntregado nchar(2) = 'ET',
	@idEstadoOrdenPagado nchar(2) = 'PA',
	@idEstadoOrdenConErrores nchar(2) = 'CE';

SELECT DISTINCT
	ob.IdSociedad,
	s.Abreviacion as SociedadCorto
FROM
	OrdenBancaria ob
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenPreAprobado, @idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
	and ob.IdUsuario = @idUsuario
ORDER BY
	s.Abreviacion ASC;


SELECT DISTINCT
	ob.IdBanco,
	b.Abreviacion as BancoCorto
FROM
	OrdenBancaria ob
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenPreAprobado, @idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
	and ob.IdUsuario = @idUsuario
ORDER BY
	b.Abreviacion ASC;


SELECT DISTINCT
	ob.IdTipoOrden,
	o.Abreviacion as TipoOrdenCorto
FROM
	OrdenBancaria ob
	join TipoOrden o on ob.IdTipoOrden = o.IdTipoOrden and o.Estado = 1
WHERE
	ob.IdEstadoOrden in (@idEstadoOrdenPreAprobado, @idEstadoOrdenAprobado, @idEstadoOrdenEntregado, @idEstadoOrdenPagado, @idEstadoOrdenConErrores)
	and ob.IdUsuario = @idUsuario
ORDER BY
	o.Abreviacion ASC;

END
go

CREATE PROCEDURE spw_hth_listarFiltrosOrdenesBancariasDiarias
/*************************************************************************
Nombre: spw_hth_listarFiltrosOrdenesBancariasDiarias
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarFiltrosOrdenesBancariasDiarias '000013';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6)
)
AS
BEGIN
SET NOCOUNT ON;

SELECT DISTINCT
	ob.IdSociedad,
	s.Abreviacion as SociedadCorto
FROM
	OrdenBancaria ob
	join Sociedad s on ob.IdSociedad = s.IdSociedad and s.Estado = 1
WHERE
	ob.IdUsuario = @idUsuario and CAST(ob.FechaCreacion as date) = CAST(GETDATE() as date)
ORDER BY
	s.Abreviacion ASC;


SELECT DISTINCT
	ob.IdBanco,
	b.Abreviacion as BancoCorto
FROM
	OrdenBancaria ob
	join Banco b on ob.IdBanco = b.IdBanco and b.Estado = 1
WHERE
	ob.IdUsuario = @idUsuario and CAST(ob.FechaCreacion as date) = CAST(GETDATE() as date)
ORDER BY
	b.Abreviacion ASC;


SELECT DISTINCT
	ob.IdTipoOrden,
	o.Abreviacion as TipoOrdenCorto
FROM
	OrdenBancaria ob
	join TipoOrden o on ob.IdTipoOrden = o.IdTipoOrden and o.Estado = 1
WHERE
	ob.IdUsuario = @idUsuario and CAST(ob.FechaCreacion as date) = CAST(GETDATE() as date)
ORDER BY
	o.Abreviacion ASC;

SELECT DISTINCT
	ob.IdEstadoOrden,
	eo.EstadoOrden as EstadoOrden
FROM
	OrdenBancaria ob
	join EstadoOrden eo on ob.IdEstadoOrden = eo.IdEstadoOrden and eo.Estado = 1
WHERE
	ob.IdUsuario = @idUsuario and CAST(ob.FechaCreacion as date) = CAST(GETDATE() as date)
ORDER BY
	eo.EstadoOrden ASC;

END
go

CREATE PROCEDURE spw_hth_generarCodigoToken
/*************************************************************************
Nombre: spw_hth_generarCodigoToken
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_generarCodigoToken '000013';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@token nvarchar(255) OUTPUT
)
AS
SET NOCOUNT ON;
BEGIN TRY
declare
	@idToken nchar(6) = '',
	@fecha date,
	@correlativo int = 0,
	@usuarioHostToHost nvarchar(20) = 'Web HostToHost';

BEGIN TRANSACTION

SELECT @fecha = GETDATE();

SELECT
	@token = Token
FROM
	Token
WHERE
	IdUsuario = @idUsuario and Fecha = @fecha;

IF (@token = '')
BEGIN
	SELECT @correlativo = COUNT(IdToken) FROM Token;
	SET @correlativo = @correlativo + 1;
	SELECT @idToken = RIGHT(REPLICATE('0', 6) + CAST(@correlativo as nvarchar), 6);
	SELECT @token = CONVERT(nvarchar(255), NEWID());

	INSERT INTO
		Token
		(
		IdToken,
		IdUsuario,
		Token,
		Fecha,
		UsuarioCreacion,
		FechaCreacion
		)
	VALUES
		(
		@idToken,
		@idUsuario,
		@token,
		@fecha,
		@usuarioHostToHost,
		GETDATE()
		);
END

COMMIT TRANSACTION

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION
print 'Error Código: ' + CAST(ERROR_NUMBER() as nvarchar)
print 'Error Línea: ' + CAST(ERROR_LINE() as nvarchar);
print 'Error Mensaje: ' + ERROR_MESSAGE();

END CATCH
go

CREATE PROCEDURE spw_hth_solicitarCodigoToken
/*************************************************************************
Nombre: spw_hth_solicitarCodigoToken
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_solicitarCodigoToken '000002';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6)
)
AS
SET NOCOUNT ON;
BEGIN TRY

declare
	@codigo int = 0,
	@mensaje nvarchar(200) = '',
	@correo nvarchar(50) = '',
	@token nvarchar(255) = '',
	@cuerpo nvarchar(4000) = '',
	@cuerpoHtml nvarchar(50) = 'HTML',
	@asunto nvarchar(200) = '',
	@nombrePerfil nvarchar(100) = 'Perfil Envío Correos',
	@returnCode int,
	@idCorreoEstructura nchar(6) = '000001';

BEGIN TRANSACTION

SELECT @asunto = Asunto, @cuerpo = Cuerpo FROM CorreoEstructura WHERE IdCorreoEstructura = @idCorreoEstructura;

SELECT
	@correo = Correo
FROM
	Usuario
WHERE
	IdUsuario = @idUsuario;

EXEC spw_hth_generarCodigoToken @idUsuario, @token OUTPUT;
SET @cuerpo = REPLACE(@cuerpo, '[token]', @token);

EXEC @returnCode = msdb.dbo.sp_send_dbmail
    @recipients = @correo,
    @body = @cuerpo,
	@body_format = @cuerpoHtml,
    @subject = @asunto,
    @profile_name = @nombrePerfil;

COMMIT TRANSACTION

IF (@returnCode = 0)
BEGIN
	SET @codigo = 10;
	SET @mensaje = 'Se generó satisfactoriamente el código token y se envió a su correo institucional.';
END
ELSE
BEGIN
	SET @codigo = -10;
	SET @mensaje = 'Se generó satisfactoriamente el código token, sin embargo no envió a su correo institucional.';
END

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION
print 'Error Código: ' + CAST(ERROR_NUMBER() as nvarchar)
print 'Error Línea: ' + CAST(ERROR_LINE() as nvarchar);
print 'Error Mensaje: ' + ERROR_MESSAGE();
SET @codigo = -15;
SET @mensaje = ERROR_MESSAGE(); 

END CATCH
SELECT @codigo as codigo, @mensaje as mensaje;
go

CREATE PROCEDURE spw_hth_validarCodigoToken
/*************************************************************************
Nombre: spw_hth_validarCodigoToken
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_validarCodigoToken '000002', '';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@token nvarchar(255)
)
AS
SET NOCOUNT ON;
BEGIN TRY

declare
	@codigo int = 0,
	@mensaje nvarchar(200) = '',
	@idToken nchar(6) = '',
	@fecha date;

BEGIN TRANSACTION

SELECT @fecha = GETDATE();

SELECT
	@idToken = IdToken
FROM
	Token
WHERE
	IdUsuario = @idUsuario and Token = @token and Fecha = @fecha;

COMMIT TRANSACTION

IF (@idToken = '')
BEGIN
	SET @codigo = -10;
	SET @mensaje = 'Código token incorrecto, solicitar un nuevo código token.';
END
ELSE
BEGIN
	SET @codigo = 10;
	SET @mensaje = 'Código token correcto.';
END

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION
print 'Error Código: ' + CAST(ERROR_NUMBER() as nvarchar)
print 'Error Línea: ' + CAST(ERROR_LINE() as nvarchar);
print 'Error Mensaje: ' + ERROR_MESSAGE();
SET @codigo = -15;
SET @mensaje = ERROR_MESSAGE(); 

END CATCH
SELECT @codigo as codigo, @mensaje as mensaje;
go

CREATE PROCEDURE spw_hth_listarUsuarios
/*************************************************************************
Nombre: spw_hth_listarUsuarios
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_listarUsuarios 1, 25, 0;

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@pagina int,
@filas int,
@totalRegistros int OUTPUT
)
AS
SET NOCOUNT ON;
BEGIN

declare
	@desde int = 0,
	@hasta int = 0;

SELECT @desde = CASE @pagina WHEN 1 THEN 1 ELSE ((@pagina - 1) * @filas) + 1 END;
SELECT @hasta = @desde + @filas - 1;
SELECT @totalRegistros = COUNT(IdUsuario) FROM Usuario WHERE Estado = 1;

WITH usuario (Id, IdUsuario, Usuario, ApePaterno, ApeMaterno, Nombres, Correo, Celular, Estado, UsuarioCreacion, FechaCreacion)
AS
(
	SELECT
		ROW_NUMBER() OVER(ORDER BY u.ApePaterno ASC),
		u.IdUsuario,
		au.UserName,
		u.ApePaterno,
		u.ApeMaterno,
		u.Nombres,
		u.Correo,
		u.celular,
		CASE u.Estado WHEN 1 THEN 'ACTIVO' ELSE 'INACTIVO' END,
		u.UsuarioCreacion,
		u.FechaCreacion
	FROM
		dbo.Usuario u
		join AspNetUsers au on u.IdUsuario = au.IdUsuario
	WHERE
		u.Estado = 1
)

SELECT
	IdUsuario, Usuario, ApePaterno, ApeMaterno, Nombres, Correo, Celular, Estado, UsuarioCreacion, FechaCreacion
FROM
	usuario
WHERE
	CAST(Id as int) >= @desde and CAST(Id as int) <= @hasta;

END
go

CREATE PROCEDURE spw_hth_buscarUsuarios
/*************************************************************************
Nombre: spw_hth_buscarUsuarios
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_buscarUsuarios '', 'falla', '';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@usuario nvarchar(20),
@apePaterno nvarchar(50),
@nombres nvarchar(50)
)
AS
SET NOCOUNT ON;
BEGIN

SELECT
	u.IdUsuario,
	au.UserName,
	u.ApePaterno,
	u.ApeMaterno,
	u.Nombres,
	u.Correo,
	u.celular,
	CASE u.Estado WHEN 1 THEN 'ACTIVO' ELSE 'INACTIVO' END,
	u.UsuarioCreacion,
	u.FechaCreacion
FROM
	dbo.Usuario u
	join AspNetUsers au on u.IdUsuario = au.IdUsuario
WHERE
	u.Estado = 1
	and (@usuario = '' or au.UserName like '%' + @usuario + '%')
	and (@apePaterno = '' or u.apePaterno like '%' + @apePaterno + '%')
	and (@nombres = '' or u.Nombres like '%' + @nombres + '%')
ORDER BY
	u.ApePaterno ASC;

END
go

CREATE PROCEDURE spw_hth_editarUsuario
/*************************************************************************
Nombre: spw_hth_editarUsuario
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_editarUsuario;

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@apePaterno nvarchar(50),
@apeMaterno nvarchar(50),
@nombres nvarchar(50),
@correo nvarchar(50),
@celular nvarchar(15)
)
AS
SET NOCOUNT ON;
BEGIN TRY

declare
	@codigo int = 0,
	@mensaje nvarchar(200) = '',
	@estadoActivo bit = 1;

BEGIN TRANSACTION

IF (@idUsuario = '')
BEGIN
	SET @codigo = -10;
	SET @mensaje = 'Parámetros no presentes en la petición, revisar función.';

END
ELSE
BEGIN
	UPDATE
		Usuario
	SET
		ApePaterno = @apePaterno,
		ApeMaterno = @apeMaterno,
		Nombres = @nombres,
		Correo = @correo,
		Celular = @celular
	WHERE
		IdUsuario = @idUsuario and Estado = @estadoActivo;

	SET @codigo = 10;
	SET @mensaje = 'Se editó satisfactoriamente el usuario.';
END

COMMIT TRANSACTION

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION
print 'Error Código: ' + CAST(ERROR_NUMBER() as nvarchar)
print 'Error Línea: ' + CAST(ERROR_LINE() as nvarchar);
print 'Error Mensaje: ' + ERROR_MESSAGE();
SET @codigo = -15;
SET @mensaje = ERROR_MESSAGE(); 

END CATCH
SELECT @codigo as codigo, @mensaje as mensaje;
go

CREATE PROCEDURE spw_hth_inactivarUsuario
/*************************************************************************
Nombre: spw_hth_inactivarUsuario
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_inactivarUsuario;

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6)
)
AS
SET NOCOUNT ON;
BEGIN TRY

declare
	@codigo int = 0,
	@mensaje nvarchar(200) = '',
	@estadoActivo bit = 1,
	@estadoInactivo bit = 0;

BEGIN TRANSACTION

IF (@idUsuario = '')
BEGIN
	SET @codigo = -10;
	SET @mensaje = 'Parámetros no presentes en la petición, revisar función.';

END
ELSE
BEGIN
	UPDATE
		Usuario
	SET
		Estado = @estadoInactivo
	WHERE
		IdUsuario = @idUsuario and Estado = @estadoActivo;

	SET @codigo = 10;
	SET @mensaje = 'Se inactivó satisfactoriamente el usuario.';
END

COMMIT TRANSACTION

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION
print 'Error Código: ' + CAST(ERROR_NUMBER() as nvarchar)
print 'Error Línea: ' + CAST(ERROR_LINE() as nvarchar);
print 'Error Mensaje: ' + ERROR_MESSAGE();
SET @codigo = -15;
SET @mensaje = ERROR_MESSAGE(); 

END CATCH
SELECT @codigo as codigo, @mensaje as mensaje;
go

CREATE PROCEDURE spw_hth_recuperarClave
/*************************************************************************
Nombre: spw_hth_recuperarClave
Autor: Felix Sueldo.
Fecha Creación: 03/12/2018
Ejecución:
exec spw_hth_recuperarClave '000002', 'http://localhost:5000/Publico/Reiniciar?usuario=aquispetoken=CfDJ8CMzxEMBO19Gmj7JCu6dNAoK0/lWTHFM9wB8C+yHnT4+70wrCH3Tebib4e+tDFOC0IkuTc5S4jJk/gyf0YH3ekeBlzIiN89O4gmCviG3uRmx7M9npIT3YWmSyJLSb7Db+8RUP3q6RCmEEFA9gMn88cW+SC9ZpU/s24zzIEQVVwXTxDYhbBg0IRUukqWkamzf4jNZIgKxjKaM0gGIZ143zt0PJGsSYxn6/wL4IEz5KIP5';

CONTROL DE CAMBIOS
CÓDIGO	-	PERSONA	-	FECHA		-	MOTIVO
001		-	FSV		-	04/12/2018	-	Se agrega parametro @parametro.
**************************************************************************/
(
@idUsuario nchar(6),
@enlace nvarchar(2000)
)
AS
SET NOCOUNT ON;
BEGIN TRY

declare
	@codigo int = 0,
	@mensaje nvarchar(200) = '',
	@correo nvarchar(50) = '',
	@token nvarchar(255) = '',
	@cuerpo nvarchar(4000) = '',
	@cuerpoHtml nvarchar(50) = 'HTML',
	@asunto nvarchar(200) = '',
	@nombrePerfil nvarchar(100) = 'Perfil Envío Correos',
	@returnCode int,
	@idCorreoEstructura nchar(6) = '000002';

BEGIN TRANSACTION

SELECT @asunto = Asunto, @cuerpo = Cuerpo FROM CorreoEstructura WHERE IdCorreoEstructura = @idCorreoEstructura;

SELECT
	@correo = Correo
FROM
	Usuario
WHERE
	IdUsuario = @idUsuario;

SET @cuerpo = REPLACE(@cuerpo, '[enlace]', @enlace);

EXEC @returnCode = msdb.dbo.sp_send_dbmail
    @recipients = @correo,
    @body = @cuerpo,
	@body_format = @cuerpoHtml,
    @subject = @asunto,
    @profile_name = @nombrePerfil;

COMMIT TRANSACTION

IF (@returnCode = 0)
BEGIN
	SET @codigo = 10;
	SET @mensaje = 'Se generó satisfactoriamente el código token y se envió a su correo institucional.';
END
ELSE
BEGIN
	SET @codigo = -10;
	SET @mensaje = 'Se generó satisfactoriamente el código token, sin embargo no envió a su correo institucional.';
END

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION
print 'Error Código: ' + CAST(ERROR_NUMBER() as nvarchar)
print 'Error Línea: ' + CAST(ERROR_LINE() as nvarchar);
print 'Error Mensaje: ' + ERROR_MESSAGE();
SET @codigo = -15;
SET @mensaje = ERROR_MESSAGE(); 

END CATCH
SELECT @codigo as codigo, @mensaje as mensaje;
go