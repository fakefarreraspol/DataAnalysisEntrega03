<?php
// Your database connection code here
$servername = "localhost";
$username   = "polfo";
$password   = "7uHDSDazj5TM";
$dbname     = "polfo";

$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Retrieve the last session ID from the 'damage' table
$sql = "SELECT SessionId FROM Damage ORDER BY TimeStamp DESC LIMIT 1";
$result = $conn->query($sql);

if ($result !== false) {
    // Check if any rows were returned
    if ($result->num_rows > 0) {
        // Output the last session ID
        $row = $result->fetch_assoc();
        $lastSessionId = $row["SessionId"] + 1;
        echo $lastSessionId;
    } else {
        // Set the session ID to 0 when no session IDs are found
        echo "0";
    }
} else {
    // Handle the case when the query fails
    echo "Error executing the query: " . $conn->error;
}

// Close the database connection
$conn->close();
?>