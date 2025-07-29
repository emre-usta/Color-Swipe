# Color Wheel Challenge - Unity 2D Mobile Game

A fun and challenging 2D mobile game built with Unity where players must match colors on a spinning wheel. Test your reflexes and precision as the wheel spins faster with each successful match!

## 🎮 Game Features

- **Spinning Color Wheel**: Interactive 6-segment color wheel with smooth rotation mechanics
- **Touch Controls**: Optimized swipe-to-spin controls for mobile devices
- **Progressive Difficulty**: Wheel spins faster after each correct match
- **Score System**: Earn points for correct matches, with increasing rewards per level
- **Mobile-Optimized**: Built specifically for touchscreen devices (Android/iOS)
- **Clean UI**: Simple and intuitive user interface with score display and game controls
- **Audio Feedback**: Sound effects for enhanced gameplay experience

## 🎯 How to Play

1. A target color is displayed at the top of the screen
2. Swipe left or right on the screen to spin the color wheel
3. Try to stop the wheel so the target color is at the top
4. Correct matches earn points and increase the difficulty
5. Wrong matches end the game
6. Try to achieve the highest score possible!

## 🛠 Project Structure

```
Assets/
├── Scripts/
│   ├── ColorWheelGame.cs      # Main game controller
│   ├── ColorWheel.cs          # Wheel rotation and input handling
│   ├── WheelSegment.cs        # Individual wheel segment component
│   ├── TouchInputHandler.cs   # Enhanced touch input management
│   └── AudioManager.cs        # Audio system management
├── Scenes/
│   └── GameScene.unity        # Main game scene
└── ProjectSettings/
    └── ProjectSettings.asset  # Mobile-optimized project settings
```

## 🔧 Setup Instructions

### Prerequisites
- Unity 2020.3 LTS or newer
- Android Studio (for Android builds)
- Xcode (for iOS builds, macOS only)

### Installation

1. **Clone or Download the Project**
   ```bash
   git clone <repository-url>
   cd color-wheel-challenge
   ```

2. **Open in Unity**
   - Launch Unity Hub
   - Click "Open" and select the project folder
   - Unity will automatically import and set up the project

3. **Setup for Mobile Development**
   
   **For Android:**
   - Go to File → Build Settings
   - Select "Android" platform
   - Click "Switch Platform"
   - Install Android Build Support if prompted
   - Configure Android SDK path in Edit → Preferences → External Tools

   **For iOS:**
   - Go to File → Build Settings  
   - Select "iOS" platform
   - Click "Switch Platform"
   - Install iOS Build Support if prompted

### 🎮 Building the Game

#### Android Build
1. File → Build Settings → Android
2. Click "Player Settings" and configure:
   - Company Name: ColorWheelStudio
   - Product Name: Color Wheel Challenge
   - Bundle Identifier: com.colorwheelstudio.colorwheelchallenge
3. Click "Build" and select output location

#### iOS Build
1. File → Build Settings → iOS
2. Click "Player Settings" and configure bundle identifier
3. Click "Build" to generate Xcode project
4. Open generated Xcode project and build for device

## 🎨 Game Design

### Color Wheel System
- **6 Segments**: Red, Blue, Green, Yellow, Magenta, Cyan
- **Physics-Based Rotation**: Realistic spinning with friction
- **Touch-Responsive**: Direct manipulation through swipe gestures

### Gameplay Mechanics
- **Target Color Selection**: Random color selection for each round
- **Color Matching**: Precise color detection when wheel stops
- **Progressive Speed**: Base speed increases by 20 units per level
- **Scoring System**: Points = Level × 10 per correct match

### Mobile Optimization
- **Touch Input**: Supports both touch and mouse input for testing
- **Performance**: Optimized for 60 FPS on mobile devices
- **UI Scaling**: Responsive UI that works on various screen sizes
- **Battery Efficient**: Minimal resource usage with static batching

## 🎵 Audio System

The game includes an `AudioManager` for:
- Wheel spinning sound effects
- Correct/incorrect match feedback
- Background music
- Game over audio
- Volume controls for SFX and music

## 🔧 Customization

### Adjusting Difficulty
Edit `ColorWheelGame.cs`:
```csharp
public float baseSpinSpeed = 100f;        // Initial wheel speed
public float speedIncreasePerLevel = 20f; // Speed increase per level
```

### Modifying Colors
Edit the `wheelColors` array in `ColorWheelGame.cs`:
```csharp
private Color[] wheelColors = {
    Color.red,
    Color.blue,
    Color.green,
    Color.yellow,
    Color.magenta,
    Color.cyan
};
```

### Touch Sensitivity
Adjust in `ColorWheel.cs`:
```csharp
public float swipeSensitivity = 2f;      // Rotation sensitivity
public float minSwipeDistance = 50f;     // Minimum swipe distance
```

## 📱 Platform Support

- **Android**: API Level 19+ (Android 4.4)
- **iOS**: iOS 10.0+
- **Editor**: Windows, macOS, Linux (for testing)

## 🐛 Troubleshooting

### Common Issues

1. **Wheel not spinning on mobile**
   - Ensure touch input is enabled in Player Settings
   - Check that the scene has proper touch handling setup

2. **Build errors on Android**
   - Verify Android SDK is properly configured
   - Check minimum API level is set to 19 or higher

3. **Performance issues**
   - Ensure Static Batching is enabled in Player Settings
   - Check that VSync is appropriate for target platform

### Performance Tips
- Use Development Build for testing and debugging
- Enable GPU Skinning for better performance
- Optimize texture sizes for target devices

## 📄 License

This project is provided as a learning resource. Feel free to use and modify for educational purposes.

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:
1. Fork the repository
2. Create a feature branch
3. Test thoroughly on both Android and iOS
4. Submit a pull request with detailed description

---

**Enjoy playing Color Wheel Challenge!** 🎨🎯