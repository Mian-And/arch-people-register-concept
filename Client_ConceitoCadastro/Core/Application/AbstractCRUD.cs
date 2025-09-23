using System;

public abstract class AbstractCRUD
{
    /// <summary>
    /// This class is the contract that will order to module class DatabaseIntegration
    /// to make fundamentals actions on DataBase (CRUD Methods - Create, read, update and delete)
    /// </summary>
    public AbstractCRUD()
    {
        public bool CreateRegister();
        public string ReadRegister(string search);
        public bool UpdateRegister(string search);
        public bool DeleteRegister(string search);
    }
}
