CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "InspectionTypes" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(20) NOT NULL,
    CONSTRAINT "PK_InspectionTypes" PRIMARY KEY ("Id")
);

CREATE TABLE "Users" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(20) NOT NULL,
    "PasswordHash" bytea NOT NULL,
    "PasswordSalt" bytea NOT NULL,
    "Role" text NOT NULL,
    "About" text NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "Inspections" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Status" text NOT NULL,
    "Comment" character varying(200) NOT NULL,
    "InspectionTypeId" integer NOT NULL,
    "UserId" integer NOT NULL,
    CONSTRAINT "PK_Inspections" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Inspections_InspectionTypes_InspectionTypeId" FOREIGN KEY ("InspectionTypeId") REFERENCES "InspectionTypes" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Inspections_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Inspections_InspectionTypeId" ON "Inspections" ("InspectionTypeId");

CREATE INDEX "IX_Inspections_UserId" ON "Inspections" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220425004038_InitialPostgres', '6.0.4');

COMMIT;

