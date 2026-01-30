#!/bin/bash

# =============================================
# Script: setup-database.sh
# Description: Sets up SQL Server in Docker and initializes Northwind database
# =============================================

echo "=========================================="
echo "  Northwind Database Setup Script"
echo "=========================================="

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "Error: Docker is not running. Please start Docker and try again."
    exit 1
fi

echo ""
echo "Step 1: Starting SQL Server container..."
docker-compose up -d

echo ""
echo "Step 2: Waiting for SQL Server to be ready (this may take 30-60 seconds)..."
sleep 30

# Wait for SQL Server to be ready
MAX_TRIES=30
COUNTER=0
until docker exec northwind-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -Q "SELECT 1" > /dev/null 2>&1
do
    COUNTER=$((COUNTER+1))
    if [ $COUNTER -gt $MAX_TRIES ]; then
        echo "Error: SQL Server did not start in time."
        exit 1
    fi
    echo "   Waiting for SQL Server... ($COUNTER/$MAX_TRIES)"
    sleep 5
done

echo "   SQL Server is ready!"

echo ""
echo "Step 3: Creating database and tables..."
docker exec northwind-sqlserver /opt/mssql-tools/bin/sqlcmd \
    -S localhost -U sa -P "YourStrong@Passw0rd" \
    -i /scripts/01-CreateNorthwindDatabase.sql

echo ""
echo "Step 4: Inserting sample data..."
docker exec northwind-sqlserver /opt/mssql-tools/bin/sqlcmd \
    -S localhost -U sa -P "YourStrong@Passw0rd" \
    -i /scripts/02-InsertSampleData.sql

echo ""
echo "=========================================="
echo "  Setup Complete!"
echo "=========================================="
echo ""
echo "Connection String:"
echo "  Server=localhost,1433;Database=Northwind;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
echo ""
echo "To run the API:"
echo "  dotnet run --project src/NorthwindApp.Api"
echo ""
echo "To stop the database:"
echo "  docker-compose down"
echo ""
