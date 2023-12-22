<?php

include_once 'settings.php';


if(true)
{    
     $posX = $_GET['posX']; 
     $posY = $_GET['posY'];
     $posZ = $_GET['posZ'];
	 

     $sql = "INSERT INTO Damage (posX,posY,posZ)
     VALUES ('$posX' ,'$posY','$posZ')";
     if (mysqli_query($conn, $sql)) {
        echo "Damage has been added successfully! ";

        $last_id = $conn->insert_id;
        echo "New Damage record created successfully.";
     } else {
        echo "Error: " . $sql . ":-" . mysqli_error($conn);
     }
     mysqli_close($conn);
}
?>