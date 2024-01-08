<?php
$servername = "localhost";
$username   = "polfo";
$password   = "7uHDSDazj5TM";
$dbname     = "polfo";

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Check if data is received via POST
if ($_SERVER["REQUEST_METHOD"] === "POST") {
    $data = json_decode($_POST['data']);

    if ($data === null) {
        // Check if there was an error decoding the JSON data
        switch (json_last_error()) {
            case JSON_ERROR_NONE:
                echo "No data received!";
                break;
            case JSON_ERROR_DEPTH:
                echo "Maximum stack depth exceeded!";
                break;
            case JSON_ERROR_STATE_MISMATCH:
                echo "Underflow or the modes mismatch!";
                break;
            case JSON_ERROR_CTRL_CHAR:
                echo "Unexpected control character found!";
                break;
            case JSON_ERROR_SYNTAX:
                echo "Syntax error, malformed JSON!";
                break;
            case JSON_ERROR_UTF8:
                echo "Malformed UTF-8 characters, possibly incorrectly encoded!";
                break;
            default:
                echo "Unknown JSON error!";
                break;
        }
    } else {
        // Extract data from the JSON object
        $posX = $data->PosX;
        $posY = $data->PosY;
        $posZ = $data->PosZ;
        $sessionId = $data->SessionId;

        // Insert data into the database
        $sql = "INSERT INTO Jumped (PosX, PosY, PosZ, SessionId) VALUES ('$posX', '$posY', '$posZ', '$sessionId')";

        if (mysqli_query($conn, $sql)) {
            echo "Jump has been added successfully! ";
            $last_id = $conn->insert_id;
            echo "New Jump record created successfully.";
        } else {
            echo "Error: " . $sql . ":-" . mysqli_error($conn);
        }
    }
    mysqli_close($conn);
} else {
    echo "No data received!";
}
?>