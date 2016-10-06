class HitBox{
  //declare variables
  float minX, minY, maxX, maxY;    //bounding box variables
  PShape box;    //visual representation of the bounding box
  
  HitBox(float miX, float miY, float maX, float maY){
    //initialize variables
    minX = miX;
    minY = miY;
    maxX = maX;
    maxY = maY;
    
    //create the pshape
    box = createShape(RECT, 0, 0, maxX - minX, maxY - minY);
  }
  
  //check if box collides with another box
  boolean Collide(float minBoxX, float maxBoxX, float minBoxY, float maxBoxY){
    //compare this box's X to the other box's X
    if (minX < maxBoxX && maxX > minBoxX){
      //compare this box's y to other box's y
      if (minY < maxBoxY && maxY > minBoxY){
        return true;
      }
      else{
        return false;
      }
    }
    else{
      return false;
    }
  }
  
  void Display(){
    pushMatrix();
    translate(minX, minY);
    shape(box);
    popMatrix();
  }
}