
1、一个总控制对象，Game，控制save，load，create。
//2、3个预制件，3个材质。
3、create方法实现随机，根据要求创建预制对象，赋予材质，颜色
//4、实现2个IO类，reader，writer，控制数据的储存和读取，
//5、实现对象自己储存（PersistableObject）自己的保持信息。
//    实现Shape类，标记不同的形状预制件
6、实现PersistentStorage，管理对象的save和load
7、实现shapeFactory类，管理对象的创建，属性获取，共game.create使用，实现load方法
8、持久化保存加上版本控制。
9、使用MaterialPropertyBlock优化材质设置颜色


二、
1、增加删除对象功能。按X键删除
2、增加GUI，实现自动创建，删除操作，使用滑动条控制速度
3、实现对象池。

四、
1、把所有Shape放到一个Scene下，统一管理，注意scene在编译模式下的缓存问题。
2、新建一个场景Level1，实现切换到新场景，并把创建的Shape带入新场景，注意场景已经加载问题。
3、创建3个新场景，实现场景切换，玩家按数字健，切换到对应场景，并实现切换后卸载旧场景
4、

