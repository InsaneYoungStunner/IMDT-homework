# GAIL生成对抗模仿学习

1. 通过深度强化学习，我们能够让机器人针对一个任务实现从0到1的学习，但是需要我们定义出reward函数，在很多复杂任务，例如无人驾驶中，很难根据状态特征来建立一个科学合理的reward。
2. 人类学习新东西有一个重要的方法就是模仿学习，通过观察别人的动作来模仿学习，不需要知道任务的reward函数。模仿学习就是希望机器能够通过观察模仿专家的行为来进行学习。
3. OpenAI，DeepMind，Google Brain目前都在向这方面发展。

### 模仿学习

1. 从给定的专家轨迹中进行学习。
2. 机器在学习过程中能够跟环境交互，到那时不能直接获得reward。
3. 在任务中很难定义合理的reward（自动驾驶中撞人reward，撞车reward，红绿灯reward），人工定义的reward可能会导致失控行为（让agent考试，目标为考100分，但是reward可能通过作弊的方式）。
4. 三种方法：
   a. 行为克隆（Behavior Cloning）
   b. 逆向强化学习（Inverse Reinforcement Learning）
   c. GAN引入IL（Generative Adversarial Imitation Learning）
5. 行为克隆
   有监督的学习，通过大量数据，学习一个状态s到动作a的映射

![在这里插入图片描述](https://img-blog.csdn.net/20180928165547860?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl8zNzg5NTMzOQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

但是专家轨迹给定的数据集是有限的，无法覆盖所有可能的情况。如果更换数据集可能效果会不好。则只能不断增加训练数据集，尽量覆盖所有可能发生的状态。但是并不实际，在很多危险状态采集数据成本非常高。

6.逆向强化学习
RL是通过agent不断与environment交互获取reward来进行策略的调整，最终得到一个optimal policy。但IRL计算量较大，在每一个内循环中都跑了一遍RL算法。



1. ![在这里插入图片描述](https://img-blog.csdn.net/20180928165603170?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl8zNzg5NTMzOQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
   IRL不同之处在于，无法获取真实的reward函数，但是具有根据专家策略得到的一系列轨迹。假设专家策略是真实reward函数下的最优策略，IRL学习专家轨迹，反推出reward函数。
   ![在这里插入图片描述](https://img-blog.csdn.net/20180928165616470?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl8zNzg5NTMzOQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
   得到复原的reward函数后，再进行策略函数的估计。
   RL算法：
   ![在这里插入图片描述](https://img-blog.csdn.net/20180928165630518?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl8zNzg5NTMzOQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
   IRL算法：
   ![在这里插入图片描述](https://img-blog.csdn.net/20180928165641310?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl8zNzg5NTMzOQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
   在给定的专家策略后（expert policy），不断寻找reward function来使专家策略是最优的。（解释专家行为，explaining expert behaviors）。具体流程图如下：
   ![在这里插入图片描述](https://img-blog.csdn.net/201809281656521?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl8zNzg5NTMzOQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

   7.生成对抗模仿学习（GAN for Imitation Learning）

   简单回顾下GAN，在GAN中，我们有Generator和Discriminator。其最初主要应用于图像生成，因此我们以图像生成这一应用来介绍下它的主要流程：在图像生成中，Generator要用来学习真实图像分布从而让自身生成的图像更加真实，以骗过Discriminator。Discriminator则需要对接收的图片进行真假判别。在整个过程中，Generator努力地让生成的图像更加真实，而Discriminator则努力地去识别出图像的真假，这个过程相当于一个二人博弈，随着时间的推移，Generator和Discriminator在不断地进行对抗，最终两个网络达到了一个动态均衡：Generator生成的图像接近于真实图像分布，而Discriminator识别不出真假图像，对于给定图像的预测为真的概率基本接近 0.5（相当于随机猜测类别）。

   回到GAIL中，我们该如何应用GAN的思想呢。在这之前，我们首先要做出假设，即我们已有的高手的策略就是最优策略，在不同的状态s下所采取的动作a就是最优的动作。

   在GAIL中，Generator其实就是我们的Agent，它会根据不同的state，采取不同的动作。而Discriminator将要努力区分高手的行动和agent的行动。**对Discriminator来说，我们可以转化成一个简单的二分类问题，即将当前的状态和动作作为输入，得到这个动作是最优动作的概率。如果这个状态-动作对来自高手的交互样本，那么Discriminator希望得到的概率越接近于1越好，而如果这个状态-动作对来自Generator的交互样本，那么Discriminator希望得到的概率越接近于0越好**。**对Generator来说，我们希望自己的策略越接近于高手的策略，那么就可以使用Discriminator输出的概率作为奖励，来更新自身的策略，如果Discriminator给出的概率越高，说明我们在这一状态下采取的动作是一个较优的动作，我们就提高该动作出现的概率，反之则是一个较差的动作，降低其出现的概率**。

   我们可以假设专家轨迹是属于某一分布（distribution），我们想让我们的模型也去预测一个分布，并且使这两个分布尽可能的接近。
   ![在这里插入图片描述](https://img-blog.csdn.net/20180928165705459?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl8zNzg5NTMzOQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
   算法流程如下：
   ![在这里插入图片描述](https://img-blog.csdn.net/20180928165716129?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl8zNzg5NTMzOQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
   Discriminator：尽可能的区分轨迹是由expert生成还是Generator生成。
   ![在这里插入图片描述](https://img-blog.csdn.net/20180928165727717?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl8zNzg5NTMzOQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
   Generator(Actor)：产生出一个轨迹，使其与专家轨迹尽可能相近，使Discriminator无法区分轨迹是expert生成的还是Generator生成的。
   ![在这里插入图片描述](https://img-blog.csdn.net/20180928165743609?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl8zNzg5NTMzOQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
   其算法可以写为：

   ​

   ![在这里插入图片描述](https://img-blog.csdn.net/20180928170114851?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl8zNzg5NTMzOQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

   ​