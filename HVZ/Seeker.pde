//Seeker class
//Creates an inherited Seeker object from the Vehicle class
//Due to the abstract nature of the vehicle class, the Seeker
//  must implement the following methods:  
//  calcSteeringForces() and display()

class Seeker extends Vehicle {

  //---------------------------------------
  //Class fields
  //---------------------------------------

  //seeking target
  //set to null for now
  PVector target = null;

  //PShape to draw this seeker object
  PImage body;

  //overall steering force for this Seeker accumulates the steering forces
  //  of which this will be applied to the vehicle's acceleration
  PVector steeringForce;
  Vehicle closesthuman;  //position of closest human
  boolean seekCenter;    //should we seek the center?
  PVector seekingForce;


  //---------------------------------------
  //Constructor
  //Seeker(x position, y position, radius, max speed, max force)
  //---------------------------------------
  Seeker(float x, float y, float r, float ms, float mf) {
    //call the super class' constructor and pass in necessary arguments
    super(x, y, r, ms, mf);

    //instantiate steeringForce vector to (0, 0)
    steeringForce = new PVector(0, 0);
    seekingForce = new PVector(0, 0);

    //PImage initialization
    int imgNum = (int) random(3);
    if (imgNum == 0){
      body = loadImage("Mechon-1.png");
    }
    else if (imgNum == 1){
      body = loadImage("Mechon-2.png");
    }
    else{
      body = loadImage("Mechon-3.png");
    }
    body.resize((int) r, (int) r);
    /*
    body.beginShape();
     body.vertex(30, 20);
     body.vertex(0, 0);
     body.vertex(15, 20);
     body.vertex(0, 40);
     body.vertex(30, 20);
     body.endShape();
     //draw the seeker "pointing" toward 0 degrees*/

    //initialize variables
    seekCenter = false;
    closesthuman = null;
    mass = 2;
  }


  //--------------------------------
  //Abstract class methods
  //--------------------------------

  //Method: calcSteeringForces()
  //Purpose: Based upon the specific steering behaviors this Seeker uses
  //         Calculates all of the resulting steering forces
  //         Applies each steering force to the acceleration
  //         Resets the steering force
  void calcSteeringForces() {
    //get the steering force returned from calling seek
    float distance = 0;
    float closestDistance = Float.MAX_VALUE;
    for (int i = 0; i < human.size(); i++) {
      //get the distance from me to the human
      distance = PVector.dist(human.get(i).position, position);
      //check if this human is the closest human
      if (distance < closestDistance) {
        closestDistance = distance;
        closesthuman = human.get(i);
      }
    }
    
    if (closesthuman != null){
      seekingForce = super.pursue(closesthuman);
    }

    //get the avoidance force
    PVector avoidForce = new PVector(0, 0);
    for (int i = 0; i < obst.size(); i++) {
      avoidForce.add(PVector.mult(super.avoidObstacle(obst.get(i), 100), 1.3));
    }

    //add the above seeking force to this overall steering force
    steeringForce.add(PVector.mult(seekingForce, .75));
    steeringForce.add(avoidForce);

    //keep in the boundaries
    if (position.x < 100 || position.y < 100 || position.x > width - 100 || position.y > width - 100) {
      seekCenter = true;
    } else if (position.x > 150 && position.x < width - 150 && position.y > 150 && position.y < width - 150) {
      seekCenter = false;
    }
    if (seekCenter) {
      PVector centeringForce = super.seek(new PVector(width / 2, height / 2));
      steeringForce.add(PVector.mult(centeringForce, 1.01));
    }

    //limit this seeker's steering force to a maximum force
    steeringForce.limit(super.maxForce);

    //apply this steering force to the vehicle's acceleration
    acceleration.add(steeringForce);

    //reset the steering force to 0
    steeringForce.mult(0);
  }


  //Method: display()
  //Purpose: Finds the angle this seeker should be heading toward
  //         Draws this seeker as a triangle pointing toward 0 degreed
  //         All Vehicles must implement display
  void display() {
    //calculate the direction of the current velocity - this is done for you
    float angle = velocity.heading();   

    //draw this vehicle's body PShape using proper translation and rotation
    pushMatrix();
    translate(position.x, position.y);
    rotate(angle);
    image(body, -20, -20);
    popMatrix();

    //debug lines
    if (debuggin){
      pushMatrix();
      translate(position.x, position.y);
      stroke(color(0, 255, 255));
      line(0, 0, right.x * 50, right.y * 50);
      stroke(color(0, 0, 0));
      line(0, 0, forward.x * 50, forward.y * 50);
      stroke(color(255, 0, 0));
      if (closesthuman != null){
        line(0, 0, closesthuman.position.x - position.x, closesthuman.position.y - position.y);
        stroke(color(50, 50, 50));
      }
      line(0, 0, seekingForce.x * 50, seekingForce.y * 50);
      popMatrix();
    }
  }

  //--------------------------------
  //Class methods
  //--------------------------------
}