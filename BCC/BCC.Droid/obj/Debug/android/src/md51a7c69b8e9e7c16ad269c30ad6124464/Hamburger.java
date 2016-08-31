package md51a7c69b8e9e7c16ad269c30ad6124464;


public class Hamburger
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("BCC.Droid.Views.Hamburger, BCC.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Hamburger.class, __md_methods);
	}


	public Hamburger () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Hamburger.class)
			mono.android.TypeManager.Activate ("BCC.Droid.Views.Hamburger, BCC.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
