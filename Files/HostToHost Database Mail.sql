
use master
go
-- Enable database mail functionality.
exec sp_configure 'show advanced options', 1
reconfigure
exec sp_configure 'Database Mail XPs', 1
reconfigure

-- Initial cleanup
declare
	@ProfileName nvarchar(50) = 'Perfil Envío Correos',
	@AccountName nvarchar(100) = 'Cuenta HostToHost',
	@Description nvarchar(100) = 'Cuenta de correo para envío de tokens de autorización para la aprobación de ordenes bancarias.',
	@EmailAddress nvarchar(100) = 'aplicacionesnosap@grupogloria.com.pe',
	@ReplyToAddress nvarchar(100) = '',
	@DisplayName nvarchar(50) = 'HostToHost',
	@MailServerName nvarchar(50) = '10.2.10.203',
	@UserName nvarchar(50) = 'aplicacionesnosap@grupogloria.com.pe',
	@Puerto int = 25;
	--@Password nvarchar(50) = 'inicio002';

IF EXISTS(
SELECT * FROM msdb.dbo.sysmail_profileaccount pa
      JOIN msdb.dbo.sysmail_profile p ON pa.profile_id = p.profile_id
      JOIN msdb.dbo.sysmail_account a ON pa.account_id = a.account_id
WHERE
      p.name = @ProfileName AND
      a.name = @AccountName)
BEGIN
      PRINT 'Deleting Profile Account'
      EXECUTE msdb.dbo.sysmail_delete_profileaccount_sp
      @profile_name = @ProfileName,
      @account_name = @AccountName
END
 
IF EXISTS(
SELECT * FROM msdb.dbo.sysmail_profile p
WHERE p.name = @ProfileName)
BEGIN
      PRINT 'Deleting Profile.'
      EXECUTE msdb.dbo.sysmail_delete_profile_sp
      @profile_name = @ProfileName
END
 
IF EXISTS(
SELECT * FROM msdb.dbo.sysmail_account a
WHERE a.name = @AccountName)
BEGIN
      PRINT 'Deleting Account.'
      EXECUTE msdb.dbo.sysmail_delete_account_sp
      @account_name = @AccountName
END

-- Setting up accounts and profiles
EXECUTE msdb.dbo.sysmail_add_account_sp
    @account_name = @AccountName,
    @description = @Description,
    @email_address = @EmailAddress,
    @replyto_address = @ReplyToAddress,
    @display_name = @DisplayName,
    @mailserver_name = @MailServerName,
    @port = @Puerto,
    @username = @UserName,
    --@password = @Password,
    @enable_ssl = 0;

-- Create a Database Mail profile
EXECUTE msdb.dbo.sysmail_add_profile_sp
    @profile_name = @ProfileName,
    @description = @Description;
 
-- Add the account to the profile
EXECUTE msdb.dbo.sysmail_add_profileaccount_sp
    @profile_name = @ProfileName,
    @account_name = @AccountName,
    @sequence_number = 1;
 
-- Grant access to the profile to the DBMailUsers role
EXECUTE msdb.dbo.sysmail_add_principalprofile_sp
    @profile_name = @ProfileName,
    @principal_name = 'public',
    @is_default = 0;

--Send mail
EXEC msdb.dbo.sp_send_dbmail
    @recipients=N'felix.sueldo@centro.com.pe',
    @body= 'Test Email Body',
    @subject = 'Test Email Subject',
    @profile_name = 'Perfil Envío Correos';

/*
--Profiles
SELECT * FROM msdb.dbo.sysmail_profile
 
--Accounts
SELECT * FROM msdb.dbo.sysmail_account
 
--Profile Accounts
select * from msdb.dbo.sysmail_profileaccount
 
--Principal Profile
select * from msdb.dbo.sysmail_principalprofile

--Mail Server
SELECT * FROM msdb.dbo.sysmail_server
SELECT * FROM msdb.dbo.sysmail_servertype
SELECT * FROM msdb.dbo.sysmail_configuration
 
--Email Sent Status
SELECT * FROM msdb.dbo.sysmail_allitems ORDER BY send_request_date DESC;
SELECT * FROM msdb.dbo.sysmail_sentitems ORDER BY send_request_date DESC;
SELECT * FROM msdb.dbo.sysmail_unsentitems ORDER BY send_request_date DESC;
SELECT * FROM msdb.dbo.sysmail_faileditems ORDER BY send_request_date DESC;
 
--Email Status
SELECT
	SUBSTRING(fail.subject,1,25) AS 'Subject',
    fail.mailitem_id,
    LOG.description
FROM
	msdb.dbo.sysmail_event_log LOG
	join msdb.dbo.sysmail_faileditems fail
	ON fail.mailitem_id = LOG.mailitem_id
WHERE
	event_type = 'error'
ORDER BY
	send_request_date DESC;
 
--Mail Queues
EXEC msdb.dbo.sysmail_help_queue_sp
 
--DB Mail Status
EXEC msdb.dbo.sysmail_help_status_sp
*/