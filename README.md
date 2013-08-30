## This is an add-in for [Fody](https://github.com/Fody/Fody/) 

![Icon](https://raw.github.com/philippdolder/EnableFaking.Fody/master/Icons/package_icon.png)

[Introduction to Fody](http://github.com/Fody/Fody/wiki/SampleUsage)

Change class members to `virtual` as part of your build to help making TDD less effort.

Now you ask:
> There is [Virtuosity](https://github.com/Fody/Virtuosity/) already. Why would I use **EnableFaking** instead?"

The answer: 
> EnableFaking intends to only make those types fakeable (make members `virtual`) that you otherwise would need to define an interface solely to be able to use the type with a fake framework like [Fake It Easy](https://github.com/FakeItEasy/FakeItEasy).

Instead of 
```
    public class MyImplementation : IMyInterface
    {
        public void MyMethod()
        {
        }
    }
    
    public interface IMyInterface
    {
        void MyMethod();
    }
```
you only have to write
```
    public class MyImplementation
    {
        public void MyMethod()
        {
        }
    }
```
and all other classes remain untouched.

# Nuget package

There is a nuget package available here http://nuget.org/packages/EnableFaking.Fody 

# What does it do?
### Selects all classes meeting the following criteria
  * `public`
  * not a container (has only constructors and properties)
  * not implementing any interfaces (I suggest faking the interfaces in these cases instead of the class)
  * not `sealed`
  * not `nested`

### Selects all members from the selected types that are
  * `public`
  * not `static`
  * not `virtual`
  * not a `constructor`

and changes them to `virtual`.

### All calls to the now `virtual` members are changed to virtual calls.

### If EnableFaking does not virtualize a `public` class you can add a `DoVirtualizeAttribute` on your class.
Define the attribute anywhere in your code (your code does not have a dependency to EnableFaking)
```
    [AttributeUsage(AttributeTargets.Class)]
    public class DoVirtualizeAttribute : Attribute
    {
    }
```

Add it to the class you want to be virtualized
```
    [DoVirtualize]
    public class YourClass
    {
    }
```
