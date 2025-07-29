# Color Wheel Challenge - Quick Setup Guide

## 🚀 Getting Started in Unity

### Step 1: Open the Project
1. Open Unity Hub
2. Click "Add" → "Add project from disk"
3. Select this project folder
4. Click "Open" (Unity will import all assets automatically)

### Step 2: Scene Setup
1. Open `Assets/Scenes/GameScene.unity`
2. The scene should load with a basic camera setup

### Step 3: Create the Game Objects

#### 3.1 Create the Color Wheel
1. **Create Empty GameObject**: Right-click in Hierarchy → Create Empty → Name it "ColorWheel"
2. **Add ColorWheel Script**: Select the ColorWheel object → Add Component → ColorWheel
3. **Create Wheel Visual**: 
   - Right-click ColorWheel → UI → Image → Name it "WheelContainer"
   - Set Image Source to a circle sprite (or create a white circle)
   - Position at center (0, 0, 0)

#### 3.2 Create Wheel Segments
For each of the 6 segments:
1. **Create Segment**: Right-click WheelContainer → UI → Image → Name it "Segment_1" (2, 3, etc.)
2. **Add WheelSegment Script**: Add Component → WheelSegment
3. **Position Segments**: Arrange in a circle (60° apart)
4. **Assign to Array**: Drag each segment to the ColorWheel's "Segments" array

#### 3.3 Create UI Canvas
1. **Create Canvas**: Right-click in Hierarchy → UI → Canvas
2. **Set Canvas to Screen Space - Overlay**
3. **Add Canvas Scaler**: Canvas Scaler component with UI Scale Mode = "Scale With Screen Size"
4. **Set Reference Resolution**: 1080 x 1920 (portrait mobile)

#### 3.4 Create UI Elements
Create these UI elements as children of the Canvas:

**Gameplay UI Panel:**
- Text: Score display (top-left)
- Text: Level display (top-left, below score)
- Text: Target color name (top-center)
- Image: Target color display (top-center, below text)
- Button: Pause button (top-right)

**Game Over Panel:**
- Panel: Background with semi-transparent black
- Text: "Game Over" title
- Text: Final score
- Text: High score
- Button: Restart
- Button: Main Menu

**Pause Panel:**
- Panel: Background with semi-transparent black
- Text: "Paused" title
- Button: Resume
- Button: Restart
- Button: Main Menu
- Slider: SFX Volume
- Slider: Music Volume

### Step 4: Create Game Manager
1. **Create Empty GameObject**: Name it "GameManager"
2. **Add Scripts**: 
   - ColorWheelGame
   - GameUI
   - AudioManager
   - MobileOptimizer

### Step 5: Wire Up Components

#### 5.1 ColorWheelGame Script
- **Game UI**: Drag the GameManager to the GameUI field
- **Color Wheel**: Drag the ColorWheel object

#### 5.2 GameUI Script
Assign all the UI elements you created to their respective fields in the GameUI script.

#### 5.3 ColorWheel Script
- **Wheel Transform**: Drag the WheelContainer
- **Segments**: Drag all 6 segment objects to the array

#### 5.4 AudioManager Script
- **SFX Source**: Add AudioSource component and assign
- **Music Source**: Add another AudioSource component and assign
- Import audio clips and assign to the AudioClip fields

### Step 6: Configure for Mobile

#### 6.1 Player Settings
1. **File → Build Settings → Player Settings**
2. **Company Name**: ColorWheelStudio
3. **Product Name**: Color Wheel Challenge
4. **Default Orientation**: Portrait
5. **Bundle Identifier**: com.colorwheelstudio.colorwheelchallenge

#### 6.2 Quality Settings
1. **Edit → Project Settings → Quality**
2. Set appropriate quality levels for mobile
3. The MobileOptimizer script will handle runtime optimizations

### Step 7: Test the Game
1. **Play in Editor**: Test with mouse input
2. **Build and Test**: 
   - Android: File → Build Settings → Android → Build
   - iOS: File → Build Settings → iOS → Build

## 🔧 Quick Troubleshooting

### UI Not Responsive
- Check Canvas Render Mode is "Screen Space - Overlay"
- Ensure Canvas Scaler is set to "Scale With Screen Size"
- Verify GraphicRaycaster is on the Canvas

### Touch Input Not Working
- Ensure Input System is set to "Input Manager (Old)" in Player Settings
- Check that multitouch is disabled in MobileOptimizer
- Test with Unity Remote for mobile testing

### Performance Issues
- Enable Static Batching in Player Settings
- Use Development Build to profile performance
- Check MobileOptimizer settings

### Audio Not Playing
- Verify Audio Sources are assigned and enabled
- Check volume levels in AudioManager
- Ensure audio clips are imported correctly

## 📱 Mobile Testing

### Unity Remote
1. Install Unity Remote app on your device
2. Connect device via USB
3. Enable Developer Options and USB Debugging (Android)
4. Use Edit → Project Settings → XR Plug-in Management → Unity Remote

### Build Testing
1. **Android**: Ensure Android SDK and NDK are installed
2. **iOS**: Requires Xcode on macOS
3. **Test on actual devices** for accurate performance and touch input

## 🎮 Customization Tips

### Changing Colors
Edit the `wheelColors` array in `ColorWheelGame.cs`:
```csharp
private Color[] wheelColors = {
    Color.red,        // Segment 1
    Color.blue,       // Segment 2
    Color.green,      // Segment 3
    Color.yellow,     // Segment 4
    Color.magenta,    // Segment 5
    Color.cyan        // Segment 6
};
```

### Adjusting Difficulty
Modify these values in `ColorWheelGame.cs`:
- `baseSpinSpeed`: Starting wheel speed
- `speedIncreasePerLevel`: Speed increase per level
- `stopThreshold`: How precisely player must stop

### Touch Sensitivity
Adjust in `ColorWheel.cs`:
- `swipeSensitivity`: How responsive the wheel is to swipes
- `minSwipeDistance`: Minimum swipe distance to spin wheel

---

**You're ready to spin! 🎨** The game should now be fully functional with touch controls, progressive difficulty, audio feedback, and mobile optimization.