//Vehicle class
//Specific autonomous agents will inherit from this class 
//Abstract since there is no need for an actual Vehicle object
//Implements the stuff that each auto agent needs: movement, steering force calculations, and display

abstract class Vehicle {

  //--------------------------------
  //Class fields
  //--------------------------------
  //vectors for moving a vehicle
  PVector acceleration, velocity, position, forward, right;
  
  //no longer need direction vector - will utilize forward and right
  //these orientation vectors provide a local point of view for the vehicle
  //floats to describe vehicle movement and size
  float radius, maxSpeed, maxForce, mass, wanderAngle;

  boolean debuggin;

  //--------------------------------
  //Constructor
  //Vehicle(x position, y position, radius, max speed, max force)
  //--------------------------------
  Vehicle(float x, float y, float r, float ms, float mf) {
    //Assign parameters to class fields
    velocity = new PVector(0,0);
    forward = velocity.copy().normalize();
    right = forward.copy().rotate(HALF_PI);
    acceleration = new PVector(0,0);
    position = new PVector(x, y);
    radius = r;
    maxSpeed = ms;
    maxForce = mf;
    mass = 3;
    debuggin = false;
    wanderAngle = 0;
  }

  //--------------------------------
  //Abstract methods
  //--------------------------------
  //every sub-class Vehicle must use these functions
  abstract void calcSteeringForces();
  abstract void display();

  //--------------------------------
  //Class methods
  //--------------------------------
  
  //Method: update()
  //Purpose: Calculates the overall steering force within calcSteeringForces()
  //         Applies movement "formula" to move the position of this vehicle
  //         Zeroes-out acceleration 
  void update() {
    //calculate steering forces by calling calcSteeringForces()
    calcSteeringForces();
    //add acceleration to velocity, limit the velocity, and add velocity to position
    velocity.add(acceleration);
    velocity.limit(maxSpeed);
    position.add(velocity);
    //calculate forward and right vectors
    forward = velocity.copy().normalize();
    right = forward.copy().rotate(HALF_PI);
    
    //reset acceleration
    acceleration.mult(0);
  }

  
  //Method: applyForce(force vector)
  //Purpose: Divides the incoming force by the mass of this vehicle
  //         Adds the force to the acceleration vector
  void applyForce(PVector force) {
    acceleration.add(PVector.div(force, mass));
  }
  
  
  //--------------------------------
  //Steering Methods
  //--------------------------------
  
  //Method: seek(target's position vector)
  //Purpose: Calculates the steering force toward a target's position
  PVector seek(PVector target){  
    //write the code to seek a target!
    PVector desiredVelocity = PVector.sub(target, position);
    desiredVelocity.setMag(maxSpeed);
    PVector steeringForce = desiredVelocity.sub(velocity);
    return steeringForce;
  }
  
  //Method: flee(target's position vector)
  //Purpose: Calculates the steering force away from a target's position
  PVector flee(PVector target){  
    //write the code to seek a target!
    PVector desiredVelocity = PVector.sub(position, target);
    desiredVelocity.setMag(maxSpeed);
    PVector steeringForce = desiredVelocity.sub(velocity);
    return steeringForce;
  }
  
  //method to check if 2 vehicles collide
  boolean collide(Vehicle other){
    //get distance between vehicles
    float distance = (other.position.x - position.x) * (other.position.x - position.x) + (other.position.y - position.y) * (other.position.y - position.y);
    
    //check if colliding
    if (distance < ((radius * radius) / 4 + (other.radius * other.radius / 4))){
      return true;
    }
    else{
      return false;
    }
  }
  
  //method: avoid obstacles
  //purpose: redirect the vehicle away from obstacles
  PVector avoidObstacle(Obstacle obst, float safeDist){
    //declare variables
    PVector steer = new PVector(0,0);
    PVector vecToCenter = PVector.sub(obst.position, position);
    float distance = vecToCenter.mag();
    
    //determine if we should care about obstacle
    if (distance > safeDist){
      return steer;
    }
    if (PVector.dot(vecToCenter, forward) < 0){
      return steer;
    }
    float safetyDance = PVector.dot(vecToCenter, right);
    if (abs(safetyDance) > (radius + obst.radius)){
      return steer;
    }
    
    //we need to care. calculate steering force
    //do we turn right or left? Also get the deisred velocity
    PVector desiredVelocity;
    if (safetyDance > 0){
      desiredVelocity = PVector.mult(right, -maxSpeed);
    }
    else{
      desiredVelocity = PVector.mult(right, maxSpeed);
    }
    
    //calculate the steering force
    steer = PVector.sub(desiredVelocity, velocity);
    
    //increase urgency
    steer = PVector.mult(steer, safeDist/distance);
    
    //return steering force
    return steer;
  }
  
  //Method: pursue(target's future position vector)
  //Purpose: Calculates the steering force to a target's future position position
  PVector pursue(Vehicle target){  
    //find target's future position
    PVector futurePosition = PVector.add(target.position, PVector.mult(target.velocity, 10));
    
    //calculate desired velocity and force
    PVector desiredVelocity = PVector.sub(futurePosition, position);
    desiredVelocity.setMag(maxSpeed);
    PVector steeringForce = desiredVelocity.sub(velocity);
    return steeringForce;
  }
  
  //Method: evade(target's future position vector)
  //Purpose: Calculates the steering force away from a target's future position position
  PVector evade(Vehicle target){  
    //find target's future position
    PVector futurePosition = PVector.add(target.position, PVector.mult(target.forward, 10));
    
    //calculate desired velocity and force
    
    return flee(futurePosition);
  }
  
  //Method: wander
  //Purpose: vehicle wanders about aimlessly
  PVector wander(){
    PVector wander = forward.copy();
    wander.mult(circDist);
    wanderAngle += random(-.05, .05);
    wander.add(PVector.fromAngle(wanderAngle).mult(circRad));
    return wander;
  }
}