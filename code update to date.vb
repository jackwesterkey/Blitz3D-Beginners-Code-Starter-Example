xResolution = 1200
yResolution = 675

; Initialize graphics
Graphics3D xResolution, yResolution, 0, 2
SetBuffer BackBuffer()

; Center of the screen for mouse control
centerX = xResolution * 0.5
centerY = yResolution * 0.5

; Initialize player and camera
player = CreatePivot()          ; Create player as a pivot (empty entity)
camera = CreateCamera(player)   ; Attach the camera to the player pivot

CameraViewport camera, 0, 0, xResolution, yResolution
CameraRange camera, 0.1, 150

; Set player as EntityType 1 for collisions
EntityType player, 1

; Hide the mouse pointer
HidePointer()

; Initialize cube (representing an object in the world)
cube = CreateCube()
PositionEntity cube, 0, 0, 5    ; Position cube 5 units in front of the player
EntityType cube, 2              ; Set cube entity type to 2 for collisions

; Initialize ground (plane)
plane = CreatePlane()
EntityColor plane, 127, 255, 127 ; Color the plane green
PositionEntity plane, 0, -2, 0   ; Position the plane slightly below the player
EntityType plane, 3              ; Set plane entity type to 3 for collision purposes

; Set up entity collision rules
Collisions 1, 2, 3, 2  ; EntityType 1 (player) collides with EntityType 2 (cube) and 3 (plane)

; Variables for mouse control
lastMouseX = 0
lastMouseY = 0
mX = 0
mY = 0

; Main loop
While Not KeyHit(1)

    ; Camera controls
    lastMouseX = MouseX()
    lastMouseY = MouseY()

    ; Recenter the mouse
    MoveMouse centerX, centerY

    mX = MouseX()
    mY = MouseY()

    ; Control camera pitch (up/down rotation) with mouse Y movement, limit pitch
    If EntityPitch(camera) + (lastMouseY - mY) < 40 And EntityPitch(camera) + (lastMouseY - mY) > -40
        TurnEntity camera, lastMouseY - mY, 0, 0
    EndIf

    ; Control player (and camera yaw) with mouse X movement
    TurnEntity player, 0, mX - lastMouseX, 0

    ; Movement controls
    If KeyDown(17) ; W key - move forward
        MoveEntity player, 0, 0, 0.08
    EndIf

    If KeyDown(30) ; A key - move left
        MoveEntity player, -0.08, 0, 0
    EndIf

    If KeyDown(31) ; S key - move backward
        MoveEntity player, 0, 0, -0.08
    EndIf

    If KeyDown(32) ; D key - move right
        MoveEntity player, 0.08, 0, 0
    EndIf

    ; Handle collisions (between player and other objects)
    If EntityCollided(player, 2) ; If player collides with cube (EntityType 2)
        Print "Player collided with cube!"
    EndIf

    If EntityCollided(player, 3) ; If player collides with plane (EntityType 3)
        Print "Player on ground!"
    EndIf

    ; Update the world and render the frame
    UpdateWorld
    RenderWorld
    Flip

Wend

End