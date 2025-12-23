# Chess 中国象棋软件设计

#### 介绍
喜欢象棋，也喜欢编程，所以，有了这个象棋软件。
软件设计上，借鉴了国内知名象棋软件通行的设计思想，具有友好的操作界面，符合大众使用习惯。

#### 主要功能如下：
1. 人机对战，测试自己的象棋水平。
2. 电脑对战，观看电脑控制红黑双方棋子如何攻杀。
3. 自由打谱，练习各种变化，添加着法注释，并能够全部保存。
4. 具有复盘功能，所有保存的棋谱及其着法变化，都可以随时打开进行温习。
5. 残局破解，测试残局能力。系统自带30个残局，有视频破解教程。
6. 残局设计，可不断收集、扩展残局库。

[![主菜单界面](https://gitee.com/cygsd/Chess/raw/Chess/ReadmePic/%E4%B8%BB%E8%8F%9C%E5%8D%95.png)](https://gitee.com/cygsd/Chess/raw/Chess/ReadmePic/%E4%B8%BB%E8%8F%9C%E5%8D%95.png)
[![着法提示与打分](https://gitee.com/cygsd/Chess/raw/Chess/ReadmePic/%E4%BA%BA%E6%9C%BA%E5%AF%B9%E6%88%98.png)](https://gitee.com/cygsd/Chess/raw/Chess/ReadmePic/%E4%BA%BA%E6%9C%BA%E5%AF%B9%E6%88%98.png)
[![棋谱库](https://gitee.com/cygsd/Chess/raw/Chess/ReadmePic/%E6%A3%8B%E8%B0%B1%E6%95%B0%E6%8D%AE%E5%BA%93.png)](https://gitee.com/cygsd/Chess/raw/Chess/ReadmePic/%E6%A3%8B%E8%B0%B1%E6%95%B0%E6%8D%AE%E5%BA%93.png)
[![残局破解](https://gitee.com/cygsd/Chess/raw/Chess/ReadmePic/%E6%AE%8B%E5%B1%80%E7%A0%B4%E8%A7%A3.png)](https://gitee.com/cygsd/Chess/raw/Chess/ReadmePic/%E6%AE%8B%E5%B1%80%E7%A0%B4%E8%A7%A3.png)
#### 已具备的其他功能
* 棋盘可上下翻转，红方可在下方，也可以在上方。运行期间可随意翻转棋盘。
* 走棋具备动画效果，有悔棋功能。
* 可显示棋子移动的有效位置，非法目标位置将不可走到。
* 红方先走棋，非走棋方的棋子选中无效。
* 将军时有提示，且下一步必须走解将的棋子，其他走棋无效。
* 走棋错误时，自动取消走棋，棋子返回到走棋前位置。
* 有绝杀判断功能。判断是否绝杀的算法比较复杂，费了不少脑细胞。
* 有记谱功能，可在单独窗口同步显示。
* 点“开局”按钮，可恢复到初始状态。
* 仿天天象棋界面，严格遵循象棋走棋规则。
* 使用SQLite在本地保存棋谱，具体增加、删除、修改功能。
* 完善的变招数据存储结构，可保存所有变化。
* 遇到变招时，显示箭头提示。显示箭头数量可进行设置，以便保存界面清洁。
* 电脑提示下一步最佳着法，显示局面分。
* 电脑走棋速度可人为设置。
* 窗口可任意缩放，棋盘、棋子、按钮等同步缩放。
* 窗口背景可任意更换。
* 具备界面主题选择功能，可选择个人喜好的主题。
* 自动保存用户设置，下次打开软件时，自动使用上次保存的设置。

#### 正在开发的功能，以及开发目标

1. 细化棋谱分类，至少分为象棋古谱、象棋比赛棋谱、网络教学棋谱等，如有必要则单独建数据库。
2. 开发棋谱练习功能，人、机分别执黑或执红，通过多次练习，领会、掌握棋谱的精髓。
3. 开发网络版（BS架构网页版、BC架构客户端版），通过广大网友的共同努力，扩大棋谱库数量，棋谱库将永久开放，免费共享。（目前暂时还不便实施，因为没银子租用服务器）。
4. 增加棋谱分支修剪功能，可将无效的变招分支删除。
5. 增加点击棋谱着法后，棋盘自动更新功能。
6. 开发棋谱导入功能，可收集网络上的棋谱，自动进行识别，并导入棋谱库。
7. 开发主题插件功能，用户可自定义界面样式。
8. 提供更多的棋盘、棋子样式，用户也可自行设计导入。
9. 终极目标：打造一个用户可完全掌控的、充分自由发挥的象棋软件。

#### 软件架构

编程环境：Visual Studio 2019/2022
C#，NET5.0/6.0，WPF，SQLite3.0
随着功能扩展，代码量快速增长，模块间耦合度过高问题越来越严重。下一步打算使用Prism框架对代码进行重构，重构为MVVM模式，以降低模块间的耦合度，增强可扩展性。

#### 安装教程

使用源码时，在Visual Studio中通过NuGet安装如下包：
1.  Newtonsoft.Json
2.  System.Data.SQLite
如果系统根据依赖关系自动安装了相应包，则不需要手动安装。


#### 使用说明

1.  全部源码，开箱即用。
2.  代码中含有大量注释，能够快速理解程序流程。
3.  提供预览版可执行文件，下载解压即可使用，无需安装。预览版不定期更新。

#### 代码示例

``` c#
for (int i = 0; i < 9; i++)
    for (int j = 0; j < 10; j++)
    {
        int qizi = GlobalValue.qiPan[i, j]; // 从棋盘上找到存活的本方棋子
        if (gongJiQiZi > 15 && qizi > 0 && qizi <= 15) // 黑方被将军时
        {
            thispoints = MoveCheck.GetPathPoints(qizi, GlobalValue.qiPan); // 获得本方棋子的可移动路径
            foreach (int[] point in jieShaPoints) // 逐个取出可解除将军的点位坐标
            {
                if (thispoints[point[0], point[1]] == true) // 本方棋子的可移动路径是否包含解除攻击点
                {
                    if (!MoveCheck.AfterMoveWillJiangJun(qizi, point[0], point[1], GlobalValue.qiPan))
                        return true;  // true=能够解杀
                }
            }
        }
        if (gongJiQiZi <= 15 && qizi > 16 && qizi <= 31) // 红方被将军时
        {
            thispoints = MoveCheck.GetPathPoints(qizi, GlobalValue.qiPan); // 获得本方棋子的可移动路径
            foreach (int[] point in jieShaPoints) // 逐个取出可解除将军的点位坐标
            {
                if (thispoints[point[0], point[1]] == true) // 本方棋子的可移动路径是否包含解除攻击点
                {
                    if (!MoveCheck.AfterMoveWillJiangJun(qizi, point[0], point[1], GlobalValue.qiPan))
                        return true;  // true=能够解杀
                }
            }
        }
    }
```


#### 绝杀算法流程图
[![将军绝杀算法流程图](https://gitee.com/cygsd/Chess/raw/Chess/ReadmePic/%E8%B1%A1%E6%A3%8B%E7%BB%9D%E6%9D%80%E6%B5%81%E7%A8%8B%E5%9B%BE.png)](https://gitee.com/cygsd/Chess/raw/Chess/ReadmePic/%E8%B1%A1%E6%A3%8B%E7%BB%9D%E6%9D%80%E6%B5%81%E7%A8%8B%E5%9B%BE.png)

#### 参与贡献

1.  Fork 本仓库：暂无
2.  新建 Feat_xxx 分支：暂无
3.  提交代码：暂无
4.  新建 Pull Request：暂无

#### 参考资料

1.  [opencv识别象棋棋子_中国象棋电脑应用规范——棋盘棋子的格式坐标与着法表示](https://blog.csdn.net/weixin_28681719/article/details/113090094?utm_medium=distribute.pc_relevant.none-task-blog-2~default~baidujs_title~default-4-113090094-blog-87528438.pc_relevant_paycolumn_v3&spm=1001.2101.3001.4242.3&utm_relevant_index=6)
2.  [qq象棋棋谱格式详解及其解析](https://blog.csdn.net/qq_43668159/article/details/87528438)
3.  [中国象棋棋谱棋书链接](https://blog.csdn.net/hbuxiaofei/article/details/50686325?utm_medium=distribute.pc_relevant.none-task-blog-2~default~baidujs_title~default-0-50686325-blog-87528438.pc_relevant_paycolumn_v3&spm=1001.2101.3001.4242.1&utm_relevant_index=2)
4.  [谈谈象棋的基本功《三》棋谱篇](https://blog.csdn.net/l970090853/article/details/89036756?spm=1001.2101.3001.6650.3&utm_medium=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7ERate-3-89036756-blog-87528438.pc_relevant_paycolumn_v3&depth_1-utm_source=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7ERate-3-89036756-blog-87528438.pc_relevant_paycolumn_v3&utm_relevant_index=5)

#### 中国象棋古谱

中国象棋有着悠久的历史，象棋古谱也有很多，但是流传下来的象棋谱却为数不多。
明清棋谱大致分为两大类：
一类是少林派，以橘中秘，金鹏十八变等等，简称用炮局。所谓少林派，节奏明快，直来直往，势大力沉。
一类是武当派，很简单，以梅花谱为代表，简称用马局。 所谓武当派，一波三折，曲径通幽，绵里藏针。
这里提供一些耳熟能详的古谱棋谱：
1. 《事林广记》
2. 《会珍阁》
3. 《反梅花谱》
4. 《吴氏梅花谱》
5. 《善庆堂重订梅花》
6. 《崇本堂梅花秘谱》
7. 《弃子十三刀》
8. 《奕海征帆》
9. 《心武残编》
10. 《无双品梅花谱》
11. 《桔中秘全局谱》
12. 《梅花变法谱》
13. 《梅花泉》
14. 《梅花谱》
15. 《梦入神机》
16. 《橘中秘残局谱》
17. 《江南风景》
18. 《泥马渡康王》
19. 《湖涯集》
20. 《烂柯神机》
21. 《百变象棋谱》
22. 《竹香斋象棋谱》
23. 《绿融侨》
24. 《自出洞来无敌手》
25. 《蕉窗逸品》
26. 《蕉竹斋》
27. 《适情雅趣》
28. 《金鹏十八变》
29. 《陶情逸趣》
30. 《隐秀斋象棋谱》
31. 《韬略元机全局普》
32. 《韬略元机残局谱》
33. 《马炮争雄》
34. 《三才图会》
35. 《棋谱秘录》
36. 《万宝全书》
37. 《吴绍龙象棋谱》
38. 《心武残篇》
39. 《渊深海阔象棋谱》
40. 《象棋满盘谱》
41. 《新增神妙变化象棋谱》
42. 《象棋老谱》
43. 《新镌金鹏变法象棋谱》
44. 《新选象棋谱》
45. 《效古子象棋谱》
46. 《梅花五子变》
47. 《象戏汇编》
48. 《梅花变法象棋谱》
49. 《无双品》
#### 特技

1.  感谢Gitee!
