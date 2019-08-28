package md5d5225c5ef1aff9622facb07d054ea9af;


public class AutoParkingActivity
	extends md5428c4b79253f1bbe6fb1b0fea8cb2635.BaseActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("ParkCred.Droid.Activities.AutoParkingActivity, ParkCred.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AutoParkingActivity.class, __md_methods);
	}


	public AutoParkingActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AutoParkingActivity.class)
			mono.android.TypeManager.Activate ("ParkCred.Droid.Activities.AutoParkingActivity, ParkCred.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
