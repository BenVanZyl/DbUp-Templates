/*
This is only a sample way of doing a data update
*/

Declare @data table (RowID int identity, NameValue varchar(1024))
Insert Into @data (NameValue) Values
    ('Qwerty'),
    ('Aserty'),
    ('Whatever'),
    ('Something for'),
    ('Nothing')

Insert Into NetCore.SampleTable
    (   CreateUserId,
        CreateDateTime,
        ModifyUserId,
        ModifyDateTime,
        Name
    )
    Select  -1,
            GetDate(),
            -1,
            GetDate(),
            d.NameValue
    From @data d
    Where Not Exists (Select 1 From NetCore.SampleTable Where Name =  d.NameValue)
