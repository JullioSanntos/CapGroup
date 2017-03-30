using ACE.Client.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Infrastructure
{
    public abstract class ViewModelBase : BindableNavigationalObject, ICanPersist
    {
        public ViewModelBase()
        {
            this.PropertyChanged += ViewModelBase_PropertyChanged; 
        }

        private void ViewModelBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EditingEntity) && this.EditingEntity != null)
            {
                //TODO: this will create a memory leak unless INotifyPropertyChanging is implemented
                this.EditingEntity.PropertyChanged += EditingEntity_PropertyChanged;
            }
        }

        private void EditingEntity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsDirty") RaisePropertyChanged(nameof(CanSave));
            if (e.PropertyName == "IsDirty") RaisePropertyChanged(nameof(CanCancel));
        }


        public abstract BindableObject EditingEntity { get; }

        public override string Header { get; }

        public virtual bool? CanSave => EditingEntity?.IsDirty ?? false;

        public virtual bool? CanCancel => EditingEntity?.IsDirty ?? false;

        public virtual DelegateCommand SaveCommand => new DelegateCommand((a) => Save());

        public virtual DelegateCommand CancelCommand => new DelegateCommand((a) => Cancel());

        public void Save()
        {
            this.EditingEntity.ResetIsDirty();
        }

        public void Cancel()
        {
            this.EditingEntity.Undo();
        }
    }
}
