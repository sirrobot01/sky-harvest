# **Sky Harvest** {#sky-harvest}

## **Game Design Document**

## 

## 

## 

## 

## 

## 

## 

## 

## 

## 

## 

## 

## **Table of Contents**

[Sky Harvest](#sky-harvest)

[Elevator Pitch](#elevator-pitch)

[Summary](#summary)

[Unique Selling Points (USPs)](#unique-selling-points-\(usps\))

[The Team](#the-team)

[Key Mechanics](#key-mechanics)

[Lane Switching (W/S Keys)](#lane-switching-\(w/s-keys\))

[Behavior](#behavior)

[Restrictions](#restrictions)

[Speed Control (A/D Keys)](#speed-control-\(a/d-keys\))

[Behaviour](#behaviour)

[Restrictions](#restrictions-1)

[Fruit Collection](#fruit-collection)

[Behaviour](#behaviour-1)

[Restrictions](#restrictions-2)

[Health System](#health-system)

[Behaviour](#behaviour-2)

[Time Pressure](#time-pressure)

[Behavior](#behavior-1)

[Restrictions](#restrictions-3)

[Level Progression](#level-progression)

[Behavior](#behavior-2)

[Restrictions](#restrictions-4)

[Characters and Settings](#characters-and-settings)

[The Dragon](#the-dragon)

[The City](#the-city)

[The Threats](#the-threats)

[Static](#static-obstacles)

[Moving](#moving-obstacles)

[Beginning the Game](#beginning-the-game)

[Tutorial Integration](#tutorial-integration)

[Visual Teaching](#visual-teaching)

[Guideline Text](#guideline-text)

[Walkthrough](#walkthrough)

[Start Screen and Intro Scene Setup](#start-screen-and-intro-scene-setup)

[Level 1 Walkthrough](#level-1-walkthrough)

[Next Level Preview](#next-level-preview)

[Art Style](#art-style)

[Audio](#audio)

[SWOT Analysis](#swot-analysis)

[Strengths](#strengths)

[Weaknesses](#weaknesses)

[Opportunities](#opportunities)

[Threats](#threats)

[Production Schedule](#production-schedule)

[List of All Assets](#list-of-all-assets)

[Programming Systems](#programming-systems)

[Art Assets](#art-assets)

[Audio Assets](#audio-assets)

[Level Design](#level-design)

[Prototyping](#prototyping)

## **Elevator Pitch** {#elevator-pitch}

In this fast-paced, skill-based flying adventure, guide a parent dragon through perilous city skies, dodging cannonballs and urban obstacles to collect precious fruits for hungry hatchlings.

## **Summary** {#summary}

Sky Harvest is a 2D side-scrolling action game where players control a dragon navigating through dangerous city airspace across five distinct flight lanes. The core challenge lies in balancing risk versus reward: fly fast to beat time limits but risk collision, or fly safely but potentially run out of time. Players must collect fruits of varying values while avoiding obstacles like cannonballs, building debris, and anti-aircraft fire. Each level presents escalating difficulty with tighter time constraints and more complex obstacle patterns, creating an addictive "just one more try" gameplay loop perfect for quick gaming sessions.

## **Unique Selling Points (USPs)** {#unique-selling-points-(usps)}

* The concept of the game is a similar design philosophy to that of most Nintendo games.  
* Taking a simple idea and premise in theory…  
* And winning the people over with tight gameplay and proper execution of said ideas and premises.  
* Casual players might enjoy the silly story and easy-to-pick-up auto-scrolling nature of the gameplay.  
* Hardcore players might enjoy pulling off intricate maneuvers across tightly made levels and obstacles.  
* The uniqueness of the game comes from how it caters to a variety of audiences as opposed to a specific audience.

* Unlike most autoscrolling adventures, where you can only move in set directions at a set pace…  
* The pacing of the game is dictated by you with the speed up/slow down buttons.  
* Could lead to fun level design where you have to balance time with fruit collecting, using speed up or slow down properly to achieve the best results, and deciding whether or not fruits are worth collecting  
    
* While the idea for the game doesn’t derive too far from the genre it comes from…  
* It does deviate enough with a few changes that are small on paper but much bigger in practice.  
* Precision, fast-thinking, and experimentation are rewarded, so level design using the interesting pace-changing mechanics can lead to some fun level design that’s more attentive than the usual, unintentionally generated auto-scroller.

## **The Team** {#the-team}

| Name | Role |
| :---- | :---- |
| Mukhtar Akere | Programmer |
| Christian Shamu | Artist and Designer |
| Yu Lin | Audio-Visual |

## **Key Mechanics** {#key-mechanics}

### **Lane Switching (W/S Keys)** {#lane-switching-(w/s-keys)}

#### **Behavior** {#behavior}

* Players move between **5** horizontal flight lanes  
* **W/UP** to move up  
* **S/DOWN** to move down 

#### **Restrictions** {#restrictions}

* Lane-switching takes around **0.2/0.3s** to prevent flickering

### **Speed Control (A/D Keys)** {#speed-control-(a/d-keys)}

#### **Behaviour** {#behaviour}

* **A/LEFT** key reduces speed  
* **D/Right** key increases speed.  
* The **Default** Speed is dynamic based on the level of difficulty

#### **Restrictions** {#restrictions-1}

* A slower speed means it takes more time to complete a level.

### **Fruit Collection** {#fruit-collection}

#### **Behaviour** {#behaviour-1}

* Contact with fruits adds to the level score  
* Different fruit types provide varying points  
  * Common fruits (apples): **10 points**  
  * Uncommon fruits (pineapples): **25 points**  
  * Rare fruits (golden fruits): **50 points**  
  * Health fruits(heart-shaped fruit): **1 heart bar or 50 points**

#### **Restrictions** {#restrictions-2}

* Each level requires fruit scores to unlock progression.

### **Health System**  {#health-system}

#### **Behaviour** {#behaviour-2}

* The dragon starts with 3 health points.  
* Collision with obstacles removes 1 health.  
* Zero health \= level failure

#### **Restrictions**

* If the dragon’s health bar is not filled up,   
* Contact with a Heart fruit adds one bar.

### **Time Pressure**  {#time-pressure}

#### **Behavior** {#behavior-1}

* Each level has specific time limits.  
* Failure to reach the end within the time limit results in level failure.  
* Reaching the end faster results in a tiered recognition on each level finish  
  * **Gold** for finishing in a minimum of **⅓** of the time  
  * **Silver** for finishing between ⅓ and half the time  
  * **Bronze** for finishing less than half the time

#### **Restrictions** {#restrictions-3}

* Each level has a two-minute time limit.

### **Level Progression**  {#level-progression}

#### **Behavior** {#behavior-2}

* A completed level means;  
  * The required number of fruit points has been collected.  
  * The end of the level has been reached within the specified time limit.

#### **Restrictions** {#restrictions-4}

* On failure, players will be redirected to start again from the same level with 0 point.

## **Characters and Settings** {#characters-and-settings}

### **The Dragon**  {#the-dragon}

* A protective parent seeking fruits for their offspring  
* Agile, with simple flight animations showing wing flapping intensity based on current speed.  
* **OPTIONAL**: Add a way to “shoot down” obstacles with dragonfire?

### **The City** {#the-city}

* A sprawling urban environment with industrial districts and residential areas.  
* Each area type features distinct obstacle patterns and architectural styles.  
* There’s a backdrop of the sky at the top

### **The Threats** {#the-threats}

#### **Static Obstacles** {#static-obstacles}

* Buildings  
* Fruit-shaped obstacles   
* Other obstacles

#### **Moving Obstacles** {#moving-obstacles}

* Missiles  
* Cannonballs  
* Thunder strikes

## **Beginning the Game** {#beginning-the-game}

### **Tutorial Integration** {#tutorial-integration}

No separate tutorial mode. The first level introduces mechanics progressively:

* **First 12.5%:** Learn lane switching by collecting apples without any threats.  
* **Next 12.5%:** Static obstacles(buildings) appear. Players start practicing speed control to get past them.  
* **Next 12.5%:** Slow-moving cannonballs are added. Players must avoid them by switching lanes and adjusting speed.  
* **Next 12.5%:** Missiles appear and briefly track the player’s lane, requiring faster reactions.  
* **Final 50%:** All hazards—static obstacles, cannonballs, and missiles—are active together, with more fruits to collect.

### **Visual Teaching** {#visual-teaching}

* Glowing arrows indicate optimal lane switches during the first level.  
* Fruit collection shows point values with animated text.   
* Speed changes display visual effects.

### **Guideline Text** {#guideline-text}

* Control guidelines are shown with a slight blur:  
* W/Up Arrow: UP  
* S/Down Arrow: DOWN  
* A/Left Arrow: SLOWER  
* D/Right Arrow: FASTER

## **Walkthrough** {#walkthrough}

### Start Screen and Intro Scene Setup {#start-screen-and-intro-scene-setup}

The game begins at a simple start screen displaying the **Sky Harvest** title and basic instructions. The controls (W/S or Up/Down to switch flight lanes, A/D or Left/Right to adjust speed) are subtly shown on-screen. Upon pressing “Start,” a brief story scene plays: the camera pans over a bright morning sky above a sprawling city. In a cozy rooftop nest, three baby dragons (hatchlings) are crying out for more food, their tiny voices echoing with urgency and hunger, setting a playful yet motivating tone. The parent dragon – the player’s character – spreads its wings, ready to fly. With a flap, the dragon takes off into the sky, and the first level begins, doubling as an integrated tutorial.

### **Level 1 Walkthrough** {#level-1-walkthrough}

The level is designed as a progressive tutorial, gradually introducing game mechanics within a 2:00-minute time limit. Below is the breakdown of the first level’s progression with time frames when at a normal speed:

**First 12.5%** (00:00 – 00:15)**:** The level opens calmly. A few red apples float in the starting lane, letting players learn basic lane-switching and collect points without any threats. A quick on-screen prompt may say, “Collect the apples\!” as the dragon glides past city rooftops.

**Next 12.5%** (00:15 – 00:30)**:** Static obstacles like buildings appear. Players must start adjusting speed to pass gaps or time their movement. A prompt might say, “Press A to slow down\!” Obstacles are spaced out, and fruit continues to appear in accessible lanes to reinforce safe navigation and control.

**Next 12.5%** (00:30 – 00:45)**:** Slow-moving cannonballs are introduced from construction zones or towers. A warning cue signals their arrival. Players learn to switch lanes or adjust speed to avoid them. Fruit placement guides safe movement subtly.

**Next 12.5%** (00:45 – 01:00)**:** Missiles launch from rooftops or trucks. They move faster and may follow the player’s lane briefly. Audio or visual warnings precede them. Players use speed and reflexes to dodge quickly, building on earlier mechanics.

**Final 50%** (01:00 – 02:00)**:** The final minute combines all hazards: static obstacles, cannonballs, and missiles. The patterns grow slightly denser but remain fair. More fruits appear, rewarding skilled movement. The sky brightens, and the dragon lands back at the rooftop nest, marking the end.

To complete Level 1, the player must reach the end of the level within the **2-minute** time limit and achieve a minimum score of **300 points** by collecting fruits.

### **Next Level Preview** {#next-level-preview}

Before each new level begins, the hatchlings chirp louder and demand more fruit, ramping up the challenge. Each level remains a 2-minute mission that builds on all previous mechanics. New obstacles are introduced with every level—such as thunderbolts striking in timed intervals, deceiving fruit-shaped obstacles—while previously seen hazards like missiles, cannonballs, and static structures return with greater frequency and complexity. Fruit patterns grow tighter and more punishing, testing the player's reflexes, lane-switching skills, and timing under pressure.

## **Art Style** {#art-style}

**Visual Inspiration:** Combines the vibrant, approachable style of mobile games like Alto's Adventure with the urban environment detail of Mirror's Edge. Clean, geometric city silhouettes contrast with organic dragon animations.

**Color Palette:**

* **Daytime levels:** Bright blues and whites with warm building tones  
* **Evening levels:** Orange and purple gradients with dramatic lighting  
* **Night levels:** Deep blues and purples with neon accents  
* **Dragon:** Rich greens and golds that stand out against all backgrounds

**Technical Approach:** 2D sprite-based assets with parallax scrolling backgrounds. Simple geometric shapes for obstacles ensure clear visual communication. Particle effects for fruit collection and collision feedback.

## **Audio** {#audio}

**Musical Style:** Orchestral adventure themes with electronic urban elements. The dynamic music system adjusts intensity based on the current flight speed and danger level.

**Sound Design Influences:**

* **Fruit collection**: Cheerful chime progression.  
* **Obstacle collision**: Sharp, impactful sounds without being harsh  
* **Speed changes**: Subtle audio cues (wind rushing for fast, quiet ambience for slow)  
* **Dragon wing flaps**: Rhythmic audio matching visual animation

**Implementation:** A Layered audio system where the base melody continues while additional instrument layers activate during high-speed or high-danger moments.

## **SWOT Analysis** {#swot-analysis}

### **Strengths** {#strengths}

* Simple control scheme(**WASD)** accessible to all skill levels  
* Unique speed control mechanic creates strategic depth  
* Strong visual clarity ensures a fair challenge  
* Scalable difficulty supports progressive skill building

### **Weaknesses** {#weaknesses}

* Limited mechanical variety may lead to repetition  
* Lane-based movement might feel restrictive to some players  
* Success is heavily dependent on precise timing and coordination

### **Opportunities** {#opportunities}

* Additional dragon types with unique abilities  
* Cooperative multiplayer modes (rescue missions)  
* Mobile platform adaptation potential  
* Additional varieties of cities could provide several other obstacles and tasks.

### **Threats** {#threats}

* Collision detection must be pixel-perfect to feel fair  
* Lane-switching timing needs careful calibration to prevent either a sluggish or a twitchy feel  
* Difficulty balancing requires extensive playtesting  
* Art asset creation (multiple dragon animations, diverse obstacles, varied backgrounds) may exceed available time  
* Audio synchronization complexity with variable speed gameplay

## **Production Schedule** {#production-schedule}

| Wk | Programmer | Artist | Designer | Audio |
| :---- | :---- | :---- | :---- | :---- |
| 1 | Basic movement system | Dragon concept art | Level layout planning | Audio research |
| 2 | Lane switching logic | Dragon sprite creation | Obstacle placement rules | Basic sound effects |
| 3 | Speed control implementation | Fruit asset creation | Difficulty curve testing | Background music composition |
| 4 | Collision detection | Background environment art | Level progression balancing | Audio integration |
| 5 | Scoring system | UI element design | Playtesting coordination | Sound effect polish |
| 6 | Health/time management | Visual effects creation | Tutorial flow refinement | Music mixing |
| 7 | Level progression logic | Art integration & polish | Final balancing pass | Audio implementation |
| 8 | Bug fixing & optimization | Final art assets | Documentation completion | Final audio integration |
| 9 | Build preparation | Asset organization | Submission preparation | Audio testing |
| 10 | Final testing | Polish & feedback | Final adjustments | Final delivery |

## **List of All Assets** {#list-of-all-assets}

### **Programming Systems** {#programming-systems}

| Asset | Priority | Time Estimate |
| :---- | :---- | :---- |
| Player movement controller | Essential | 3 days |
| Lane-switching system | Essential | 2 days |
| Speed control mechanics | Essential | 2 days |
| Collision detection | Essential | 4 days |
| Obstacle spawning system | Essential | 3 days |
| Fruit collection system | Essential | 2 days |
| Scoring & progression | Essential | 3 days |
| Health management | Important | 2 days |
| Timer system | Important | 1 day |
| Audio manager | Important | 2 days |
| Menu systems | Nice to have | 3 days |
| Save/load functionality | Optional | 4 days |

### **Art Assets** {#art-assets}

| Asset | Priority | Time Estimate |
| :---- | :---- | :---- |
| Dragon sprite (4 frames) | Essential | 5 days |
| Cannonball obstacles | Essential | 1 day |
| Fruit sprites (3 types) | Essential | 2 days |
| Basic background tiles | Essential | 3 days |
| UI elements | Essential | 2 days |
| Lane indicator graphics | Important | 1 day |
| Particle effects | Important | 2 days |
| Environmental details | Nice to have | 4 days |
| Additional obstacle types | Nice to have | 3 days |
| Animated backgrounds | Optional | 5 days |

### **Audio Assets** {#audio-assets}

| Asset | Priority | Time Estimate |
| :---- | :---- | :---- |
| Wing flap sound effect | Essential | 1 day |
| Fruit collection sounds | Essential | 1 day |
| Collision sound effects | Essential | 1 day |
| Background music (3 tracks) | Important | 6 days |
| UI sound effects | Important | 2 days |
| Ambient city sounds | Nice to have | 2 days |
| Dynamic music layers | Optional | 4 days |

### **Level Design** {#level-design}

| Asset | Priority | Time Estimate |
| ----- | ----- | ----- |
| Level 1 (tutorial) | Essential | 2 days |
| Level 2-5 (core gameplay) | Essential | 4 days |
| Obstacle pattern library | Essential | 3 days |
| Difficulty progression curves | Important | 2 days |
| Additional levels | Nice to have | 6 days |

**Total Essential Assets:** Approximately 45 development days 

**Total Important Assets:** An Additional 15 development days

**Risk Mitigation:** All essential assets must be completed for the minimum viable product. Important assets enhance experience, but are not required for submission

## **Prototyping** {#prototyping}

* **The bottom right image** shows a mock version of the title screen showing the big dragon main character as well as 3 little hatchlings that the big dragon is taking care of, inspired by the title screen of Yoshi’s Story.  
* **The top right image** shows how the gameplay will look normally at normal speed, the dragon in a relaxed but attentive animation, while cannonballs that need to be dodged are flying in multiple lanes that the dragon can move to  
* **The bottom left image** shows how the gameplay will look when slowed down with the dragon in a more relaxed animation as while everything else moves at a regular speed, the dragon moves slower. It also showcases static obstacles like the thunder cloud, which shoots a lightning bolt every 2 or 3 seconds, and the building.  
* **The top right image** shows how the gameplay will look when sped up, with the dragon in a more attentive animation, moving faster while everything else moves at regular speed, so the dragon is more liable to run into obstacles. The golden fruit is also shown here, a random occurrence in any stage where a fruit is worth 50% more than usual.