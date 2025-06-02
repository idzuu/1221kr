namespace _1221kr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFileSizeColumn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UploadedFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        ContentType = c.String(),
                        Content = c.Binary(),
                        UploadDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UploadedFiles");
        }
    }
}
