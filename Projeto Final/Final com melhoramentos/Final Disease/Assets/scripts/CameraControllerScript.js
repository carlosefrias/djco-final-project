#pragma strict

function Start () {
	Camera.main.transform.position.x = 110;
	Camera.main.transform.position.y = 25;
	Camera.main.transform.position.z = 90;
}

function Update () {
	if (Input.GetKey("w")) {
		camera.transform.position.z += 1.5;
	}
	if (Input.GetKey("s")) {
		camera.transform.position.z -= 1.5;
	}
	if (Input.GetKey("a")) {
		camera.transform.position.x -= 1.5;
	}
	if (Input.GetKey("d")) {
		camera.transform.position.x += 1.5;
	}
	if (Input.GetAxis("Mouse ScrollWheel") > 0) {
		if (Camera.main.fieldOfView > 30)
			Camera.main.fieldOfView -=2;
	}
	if (Input.GetAxis("Mouse ScrollWheel") < 0) {
		if (Camera.main.fieldOfView < 101)
			Camera.main.fieldOfView +=2;
	}
}

