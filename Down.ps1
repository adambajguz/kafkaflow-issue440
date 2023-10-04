# This command sets the execution policy to bypass for only the current PowerShell session after the window is closed, the next PowerShell session will open running with the default execution policy. 
# Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

docker-compose -p kafkaflow-sandbox -f docker-compose.yml down