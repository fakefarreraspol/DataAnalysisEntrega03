<?php

// Connect to your MySQL database (replace with your actual database credentials)
$servername = "localhost";
$username   = "polfo";
$password   = "7uHDSDazj5TM";
$dbname     = "polfo";

$conn = new mysqli($servername, $username, $password, $dbname);

// Check the connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Fetch data from the Damage table
$sql = "SELECT PosX, PosY, PosZ FROM Damage";
$result = $conn->query($sql);

if ($result->num_rows > 0) {

    // Prepare the damaged data as a string in the format expected by the C# function
    $damagedDataString = "";

    while ($row = $result->fetch_assoc()) {
        $damagedDataString .= $row['PosX'] . " " . $row['PosY'] . " " . $row['PosZ'] . "<br>";
    }

    // Close the database connection
    $conn->close();

    // Send the response
    echo $damagedDataString;

} else {
    // No data found in the Damage table
    echo 'No data found';
}

?>