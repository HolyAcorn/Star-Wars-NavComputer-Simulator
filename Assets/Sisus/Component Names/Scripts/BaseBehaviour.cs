using UnityEngine;

namespace Sisus.ComponentNames
{
	/// <summary>
	/// Base class from which every component class should derive to get
	/// all the functionality from the <see cref="MonoBehaviour"/> class
	/// while also having the <see cref="name"/> property and the
	/// <see cref="ToString"/> method return the actual name of the component.
	/// </summary>
	public abstract class BaseBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The name of the component.
		/// <para>
		/// In the editor this corresponds with the name of the component as shown in the Inspector.
		/// </para>
		/// <para>
		/// In builds this always returns the <see cref="System.Type.Name">name</see> of the
		/// component class and attemps to assign other values for the property are ignored.
		/// </para>
		/// </summary>
		public virtual new string name
		{
			get => this.GetName();
			set => this.SetName(value);
		}

		/// <summary>
		/// Returns the <see cref="name"/> of this component.
		/// </summary>
		/// <returns> The <see cref="name"/> of this component. </returns>
		public override string ToString() => this.GetName();
    }
}