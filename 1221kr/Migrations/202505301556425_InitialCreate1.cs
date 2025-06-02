namespace _1221kr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConsultationBookings", "MinDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ConsultationBookings", "MaxDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ConsultationBookings", "ClientId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ConsultationBookings", "ClientId", c => c.String(nullable: false));
            DropColumn("dbo.ConsultationBookings", "MaxDate");
            DropColumn("dbo.ConsultationBookings", "MinDate");
        }
    }
}
