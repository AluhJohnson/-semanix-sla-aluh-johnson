SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

create procedure [slipfree].disableSuperUser @schemaName nvarchar(20) = 'slipfree'
as
begin
    set nocount on;
    Declare @AdminCount int = 0;

    select @AdminCount = count(*)
    from [@schemaName].UserTbl a
             join [@schemaName].RoleTbl b on a.roleid = b.id
    where b.rolename = 'SUPERUSER';

    if @AdminCount >= 2
        begin
            update [@schemaName].RoleTbl set isActive = 0 where rolename = 'SUPERUSER';
            update [@schemaName].UserTbl
            set isActive        = 0,
                isAccountLocked = 1,
                DateModified    = getdate()
            where roleId = (select id from [@schemaName].RoleTbl where rolename = 'SUPERUSER')
        end
end

    SET ANSI_PADDING OFF
GO 