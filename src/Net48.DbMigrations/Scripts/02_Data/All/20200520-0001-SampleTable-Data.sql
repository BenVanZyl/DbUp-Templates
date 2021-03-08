/*	### THIS IS A SAMPLE ### */

/*
This is only a sample way of doing a data update
*/

Declare @rowId int, @rowCount int, @nameValue varchar(1024)

Declare @data table (RowID int identity, NameValue varchar(1024))
Insert Into @data (NameValue) Values
    ('Qwerty'),
    ('Aserty'),
    ('Whatever')

Select @rowId = 1, @rowCount = Count(*) From @data

While (@rowId <= @rowCount)
Begin
    Select @nameValue = NameValue
    From @data
    Where rowId = @rowId

    if Not Exists(Select 1 From Net48.SampleTable48 Where Name = @nameValue)
    Begin
        Insert Into Net48.SampleTable48
            (   CreateUserId,
                CreateDateTime,
                ModifyUserId,
                ModifyDateTime,
                Name
            )
            Values
            (   -1,
                GetDate(),
                -1,
                GetDate(),
                @nameValue
            )
    End

    Set @rowId = @rowId + 1
End