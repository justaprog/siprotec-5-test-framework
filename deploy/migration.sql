CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "TestCases" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    "DeviceFamily" text NOT NULL,
    "ProtectionFunction" text NOT NULL,
    "FaultType" text NOT NULL,
    "PickupCurrent" double precision NOT NULL,
    "FaultCurrent" double precision NOT NULL,
    "FaultStartMs" integer NOT NULL,
    "TripDelayMs" integer NOT NULL,
    "ExpectedTrip" boolean NOT NULL,
    "ExpectedTripMinMs" integer,
    "ExpectedTripMaxMs" integer,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_TestCases" PRIMARY KEY ("Id")
);

CREATE TABLE "TestRuns" (
    "Id" uuid NOT NULL,
    "TestCaseId" uuid NOT NULL,
    "Status" integer NOT NULL,
    "StartedAt" timestamp with time zone,
    "FinishedAt" timestamp with time zone,
    "ActualTrip" boolean,
    "ActualTripTimeMs" integer,
    "Passed" boolean,
    "ResultMessage" text,
    CONSTRAINT "PK_TestRuns" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_TestRuns_TestCases_TestCaseId" FOREIGN KEY ("TestCaseId") REFERENCES "TestCases" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_TestRuns_TestCaseId" ON "TestRuns" ("TestCaseId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260331175402_InitialCreate', '10.0.5');

COMMIT;

START TRANSACTION;
ALTER TABLE "TestCases" RENAME COLUMN "TripDelayMs" TO "ExpectedOutcome_TripDelayMs";

ALTER TABLE "TestCases" RENAME COLUMN "PickupCurrent" TO "Simulation_PickupCurrent";

ALTER TABLE "TestCases" RENAME COLUMN "FaultType" TO "Simulation_FaultType";

ALTER TABLE "TestCases" RENAME COLUMN "FaultStartMs" TO "Simulation_FaultStartMs";

ALTER TABLE "TestCases" RENAME COLUMN "FaultCurrent" TO "Simulation_FaultCurrent";

ALTER TABLE "TestCases" RENAME COLUMN "ExpectedTripMinMs" TO "ExpectedOutcome_ExpectedTripMinMs";

ALTER TABLE "TestCases" RENAME COLUMN "ExpectedTripMaxMs" TO "ExpectedOutcome_ExpectedTripMaxMs";

ALTER TABLE "TestCases" RENAME COLUMN "ExpectedTrip" TO "ExpectedOutcome_ExpectedTrip";

ALTER TABLE "TestCases" ALTER COLUMN "ExpectedOutcome_TripDelayMs" DROP NOT NULL;

ALTER TABLE "TestCases" ADD "Simulation_DurationMs" integer NOT NULL DEFAULT 0;

ALTER TABLE "TestCases" ADD "Simulation_NominalCurrent" double precision NOT NULL DEFAULT 0.0;

ALTER TABLE "TestCases" ADD "Simulation_SamplingRateHz" integer NOT NULL DEFAULT 0;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260419180454_RefactorTestCaseModel', '10.0.5');

COMMIT;

