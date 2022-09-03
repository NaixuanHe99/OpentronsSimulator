# Opentrons commands in log file 日志文件中的Opentrons命令

Opentrons have 12 slots on the basis: 1~11 for different equipments and 12th  is for trash.
<br>**Opentrons在此基础上有12个槽:1~11个槽用于不同的设备，12个槽用于垃圾。**

## Main component of Opentrons 主要组件

1. Tip Rack - for clean tips 
   <br>**尖端架-用于清洁尖端**
2. Reservoir - store liquid 
   <br>**储液器-储存液体**
3. Plate - 8 x 12 containers (holes/tubes) 
   <br>**板- 8 × 12容器(孔/管)**
4. Pipette - to transport a measured volume of liquid, tip needed 平板- 8 × 12容器(孔/管)移液器-运输测量体积的液体，需要尖端

## Command 命令

### Tip handling 尖端处理

A tip is needed before making liquid controls.

在进行液体控制之前需要一个尖端。

Tips can be accessed from tip rack, and would be discarded after usage.

Tips可从tip机架访问，使用后将丢弃。

#### Pick Up Tip

eg: Picking up tip from A1 of Opentrons 96 Tip Rack 300 µL on 1

从A1的Opentrons 96针尖架300µL 1上取针尖

Keywords: 'Picking up tip', 'from', 'of', 'on'

* 'from' defines the location of the tip on the rack (8 x 12, A1~H12)
  
  'from'定义了尖端在机架上的位置(8 x 12, A1~H12)

* 'of' defines the rack type

  'of'定义机架类型

* 'on' defines the slot number

  'on'定义槽号

Visual effects: 

    The pipette moves to the corresponding location and add a tip on it.

    移液管移动到相应的位置，并在上面加一个针尖。


#### Drop Tip

eg: Dropping tip into A1 of Opentrons Fixed Trash on 12

滴尖到A1的opentron固定垃圾12

Keywords: 'Dropping tip', 'into', 'of', 'on'

* 'into' defines the location in a slot

  定义槽中的位置

* 'of' defines the destination slot type

  定义目标插槽类型

* 'on' defines the slot number

  定义槽位号

Visual effects:

    The pipette moves to the corresponding location and remove its tip(s)


#### Return Tip

eg: 

Returning tip
&emsp;Returning tip into A1 of Opentrons 96 Tip Rack 300 µL on 1

将尖端放入Opentrons 96尖端架300µL的A1中

将针头放入Opentrons 96针头架的A1中，1上300µL


Keywords: 'Returning tip' + keywords from drop tip

Visual effect:

    Display 'return tip' content, apply movement based on the intended instructions (same as drop tip)

    显示“返回提示”内容，根据预期的指示应用移动(与掉落提示相同)


##### Iterating Through Tips

 Repeat tip operations, no overall prompts in logs

 重复提示操作，日志中没有整体提示

### Liquid Control

#### Aspirate

eg: Aspirating 50.0 uL from A1 of Corning 96 Well Plate 360 µL Flat on 2 at 185.72 uL/sec

以185.72微升/秒的速度从康宁96孔板的A1中吸取50.0微升，在2孔板上平放360µL

从A1号康宁96孔板360µL平板上以185.72 uL/秒的速度抽吸50.0 uL

Keywords: 'Aspirating', 'uL', 'from', 'of', 'on', 'at'

* 'uL' defines the amount of aspirated liquid

  定义吸入液体的量

* 'from' defines the aspiration location (8 x 12, A1~H12)

  定义抽吸位置(8 x 12, A1~H12)

* 'of' defines the slot type

  定义插槽类型

* 'on' defines the slot number

  定义槽位号

* 'at' defines the speed

  定义了速度

Visual effects: 

    Display the the necessary movement of the pipette and apply aspiration animation.

    Liquid in tip should appear different color.

    显示移液管的必要运动，并应用吸气动画。
    
    尖端的液体应呈现不同的颜色。


#### Dispense

eg: Dispensing 100.0 uL into A8 of NEST 96 Well Plate 200 µL Flat on 3 at 92.86 uL/sec

以92.86微升/秒的速度将100.0微升的溶液注入NEST 96孔板的A8中，在3孔板上平坦地注入200µL

以92.86 uL/秒的速度向NEST 96孔板200µL平板的A8中分配100.0 uL

Keywords: 'Dispensing', 'uL', 'into', 'of', 'on', 'at'

* 'uL' defines the amount of Dispense liquid

  定义分配液体的量

* 'into' defines the dispensing location (8 x 12, A1~H12)

  定义分配位置（8 x 12，A1~H12）

* 'of' defines the slot type

  定义插槽类型

* 'on' defines the slot number

  定义插槽编号

* 'at' defines the speed

  定义速度

Visual effect: 

    Display the necessary movement of the pipette and apply dispensation animation.

    Liquid in tip should appear different color.

    显示移液管的必要移动并应用分配动画。

    尖端中的液体应呈现不同的颜色。


#### Blow Out

eg: Blowing out at A1 of Corning 96 Well Plate 360 µL Flat on 2

在康宁96孔板的A1处吹扫，360µL平面，2

康宁96孔板360µL平板2上A1吹出

Keywords: 'Blowing out', 'at', 'of', 'on'

* 'at' defines the blow out location (8 x 12, A1~H12)

  定义吹出位置(8 x 12, A1~H12)

* 'of' defines the slot type

  定义插槽类型

* 'on' defines the slot number

  定义槽位号

Visual effect: 

    Display the necesary movement of the pipette and apply blowing out animation.

    Show that the pipette is empty.

    显示移液管必要的运动和应用吹出动画。

    显示移液管是空的。


#### Touch Tip

eg: Touching tip

Keyword: 'Touching tip'

Visual effect:

    Apply touching tip animation.

    应用触摸提示动画。

#### Mix

eg: 

Mixing 3 times with a volume of 50.0 ul

混合3次，体积为50.0 ul

&emsp;Aspirating 50.0 uL from A8 of NEST 96 Well Plate 200 µL Flat on 3 at 92.86 uL/sec

从3号NEST 96孔板200µL平板A8上以92.86 uL/秒的速度抽吸50.0 uL

&emsp;Dispensing 50.0 uL into A8 of NEST 96 Well Plate 200 µL Flat on 3 at 92.86 uL/sec

以92.86 uL/秒的速度将50.0 uL分配到NEST 96孔板200µL平板上的A8中

&emsp;Aspirating 50.0 uL from A8 of NEST 96 Well Plate 200 µL Flat on 3 at 92.86 uL/sec

从3号NEST 96孔板200µL平板A8上以92.86 uL/秒的速度抽吸50.0 uL

&emsp;Dispensing 50.0 uL into A8 of NEST 96 Well Plate 200 µL Flat on 3 at 92.86 uL/sec

以92.86 uL/秒的速度将50.0 uL分配到NEST 96孔板200µL平板上的A8中

&emsp;Aspirating 50.0 uL from A8 of NEST 96 Well Plate 200 µL Flat on 3 at 92.86 uL/sec

从3号NEST 96孔板200µL平板A8上以92.86 uL/秒的速度抽吸50.0 uL

&emsp;Dispensing 50.0 uL into A8 of NEST 96 Well Plate 200 µL Flat on 3 at 92.86 uL/sec

以92.86 uL/秒的速度将50.0 uL分配到NEST 96孔板200µL平板上的A8中


Keywords: 'Mixing', 'times', 'ul', + Aspirate & Dispense

* 'times' defines the number of Aspirate & Dispense pair

  定义送气和分配对的数量

* 'ul' defines each operation's amount

  定义每个操作的数量

Visual effect:

    Display the prompt as Mixing, and do the operations after this instructions (Aspirating and Dispensing).

    显示混合提示，并在此提示后进行操作(吸气和配药)。


#### Air Gap

eg: 

Air gap
&emsp;Aspirating 20.0 uL from B4 of Corning 96 Well Plate 360 µL Flat on 2 at 92.86 uL/sec

以92.86 uL/秒的速度从康宁96孔板360µL平板的B4上抽吸20.0 uL



Keywords:'Air gap' + Aspirate

Visual effects:

    Air gap aspirates air instead of liquid. The following action should not be a normal aspiration, instead, it should aspirate air without up-down movement.

    气隙吸入空气而不是液体。下面的动作不应该是一个正常的吸气，相反，它应该没有上下运动的吸气。


### Utility Commands

#### Delay for an Amount of Time

eg: Delaying for 0 minutes and 2.0 seconds

延迟0分零2秒

Keywords: 'Delaying', 'minutes', 'seconds'

* 'minutes' and 'seconds' define the delay time

  定义延迟时间

Visual effect:

    Apply delay and display a prompt

    应用延迟并显示提示


#### Pause Until Resumed

```csharp
protocol.pause('Time to take a break')
```

eg: Pausing robot operation: Time to take a break

暂停机器人操作:该休息一下了

It is an operation based on Opentrons App. Just display a prompt.

是基于Opentrons App的操作，只需要显示提示即可。


#### Homing

```csharp
pipette.home()  # Homes the right z axis and plunger
```

eg: Homing pipette plunger on mount left

归位移液管柱塞在左侧

Home the robot.

家庭机器人。


#### Comment

```
protocol.comment('Hello, world!')
```

eg: Hello, world!

Display this line. Any line in log mis-match any of the current operations would be a prompt.

这一行显示。日志中的任何一行与当前操作不匹配都将是一个提示。


#### Control and Monitor Robot Rail Lights

Log not supported.

日志不支持。


#### Monitor Robot Door

Log not supported.

日志不支持。


## Complex Commands

For complex commands, they are consist of a combination of basic commands, where all of them are **indented**. Keywords are the complex command operation names, and each operations would be displayed based on the following indented instructions.

对于复杂的命令，它们由基本命令的组合组成，其中所有命令都是**缩进**。关键字是复杂的命令操作名称，每个操作都将根据以下缩进的指令显示。

| Complex Commands | Keyword       | Consist of                                               |
| ---------------- | ------------- | -------------------------------------------------------- |
| Transfer         | Transferring  | Pick up tip + (Aspirate & Dispense) x n + Drop tip      |
| Consolidate      | Consolidating | Transfer (multiple aspiration combined)                  |
| Distribute       | Distributing  | Transfer (multiple dispensation combined, with blow out) |



Other parameters in Complex Command would add different sub-instructions to this operation:

复杂命令中的其他参数将向该操作添加不同的子指令：

| Parameters                  | Sub-instruction add         |
| --------------------------- | --------------------------- |
| new_tip                     | Pick up tip, Drop tip       |
| trash                       | Drop tip at trash (slot 12) |
| touch_tip                   | Touching tip                |
| blow_out / blowout_location | Blowing out                 |
| mix_before / mix_after      | Mixing                      |
| air_gap                     | Air gap                     |
| disposal_volume             | Blowing out                 |


## Notes

* Liquid in different container should be displayed clearly.

  不同容器中的液体应清楚显示。

* Use  `> logName.log` to store the output of the simulator in command line.

使用`>logName。log`将模拟器的输出存储在命令行中。
