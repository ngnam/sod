using StoreOrder.WebApplication.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace StoreOrder.WebApplication.Data.Models.Settings
{
    public class Setting
    {
        public Setting()
        {
            this.SettingGroup = (int)TypeGroupSetting.GENERAL;
            this.SettingDataType = "string";
            this.CreatedOn = DateTime.UtcNow;
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(64)]
        public string SettingKey { get; set; }
        [Required]
        [MaxLength(64)]
        public string SettingDataType { get; set; } //  /* store the datatype as string here */
        [MaxLength(1024)]
        public string SettingValueVarchar { get; set; }
        [MaxLength(500)]
        public string SettingDesc { get; set; }
        public int? SettingType { get; set; } // 1: employee, 2: customer
        public int? SettingGroup { get; set; } // Default = 1: General
        public DateTime? CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        [MaxLength(50)]
        public string LastUpdatedBy { get; set; }

        //[SystemSettingId] [int] IDENTITY NOT NULL,
        //[SettingKeyName] [nvarchar] (64) NOT NULL,
        //[SettingDataType] [nvarchar] (64) NOT NULL, /* store the datatype as string here */
        //[SettingValueBigInt] bigint NULL,
        //[SettingValueNumeric] numeric NULL,
        //[SettingValueSmallInt] smallint NULL,
        //[SettingValueDecimal] decimal NULL,
        //[SettingValueSmallMoney] smallmoney NULL,
        //[SettingValueInt] int NULL,
        //[SettingValueTinyInt] tinyint NULL,
        //[SettingValueMoney] money NULL,
        //[SettingValueFloat] float NULL,
        //[SettingValueReal] real NULL,
        //[SettingValueDate] date NULL,
        //[SettingValueDateTimeOffSet] datetimeoffset NULL,
        //[SettingValueDateTime2] datetime2 NULL,
        //[SettingValueSmallDateTime] smalldatetime NULL,
        //[SettingValueDateTime] datetime NULL,
        //[SettingValueTime] time NULL,
        //[SettingValueVarChar] varchar(1024) NULL, 
        //[SettingValueChar] char NULL,
        //[InsertDate] [datetime] NOT NULL DEFAULT(GETDATE()),               
        //[InsertedBy] [nvarchar] (50) NOT NULL DEFAULT(SUSER_SNAME()),       
        //[LastUpdated] [datetime] NOT NULL DEFAULT(GETDATE()),              
        //[LastUpdatedBy] [nvarchar] (50) NOT NULL DEFAULT(SUSER_SNAME()),   
    }
}
