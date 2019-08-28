package md5fbe61341ee493a38f48a96d6189c4cb5;


public class AutoParkingActivity
	extends md5b5879f46b95f7a4e4ffa9f18fdd8ad8a.BaseActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("ParkCred.Droid.Activities.AutoParkingActivity, ParkCred.Droid", AutoParkingActivity.class, __md_methods);
	}


	public AutoParkingActivity ()
	{
		super ();
		if (getClass () == AutoParkingActivity.class)
			mono.android.TypeManager.Activate ("ParkCred.Droid.Activities.AutoParkingActivity, ParkCred.Droid", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
