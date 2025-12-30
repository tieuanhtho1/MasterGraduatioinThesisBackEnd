-- SQL Script to Initialize Test Data for FlashCard System
-- Note: Users with Id 1 and 2 should already exist

-- =============================================
-- FlashCardCollections for User 1
-- =============================================

-- User 1: Root collection
INSERT INTO FlashCardCollections (UserId, ParentId, Title, Description)
VALUES (1, NULL, 'Programming Languages', 'Root collection for programming language topics');

-- User 1: Child collections (leaf nodes that will contain flashcards)
INSERT INTO FlashCardCollections (UserId, ParentId, Title, Description)
VALUES 
    (1, (SELECT Id FROM FlashCardCollections WHERE UserId = 1 AND Title = 'Programming Languages'), 'Python Basics', 'Fundamental Python concepts and syntax'),
    (1, (SELECT Id FROM FlashCardCollections WHERE UserId = 1 AND Title = 'Programming Languages'), 'JavaScript ES6', 'Modern JavaScript features and best practices'),
    (1, (SELECT Id FROM FlashCardCollections WHERE UserId = 1 AND Title = 'Programming Languages'), 'C# Fundamentals', 'Core C# programming concepts'),
    (1, (SELECT Id FROM FlashCardCollections WHERE UserId = 1 AND Title = 'Programming Languages'), 'SQL Queries', 'Database queries and optimization');

-- =============================================
-- FlashCardCollections for User 2
-- =============================================

-- User 2: Root collection
INSERT INTO FlashCardCollections (UserId, ParentId, Title, Description)
VALUES (2, NULL, 'Data Science & ML', 'Root collection for data science and machine learning');

-- User 2: Child collections (leaf nodes that will contain flashcards)
INSERT INTO FlashCardCollections (UserId, ParentId, Title, Description)
VALUES 
    (2, (SELECT Id FROM FlashCardCollections WHERE UserId = 2 AND Title = 'Data Science & ML'), 'Machine Learning Algorithms', 'Common ML algorithms and their applications'),
    (2, (SELECT Id FROM FlashCardCollections WHERE UserId = 2 AND Title = 'Data Science & ML'), 'Statistics Basics', 'Statistical concepts and methods'),
    (2, (SELECT Id FROM FlashCardCollections WHERE UserId = 2 AND Title = 'Data Science & ML'), 'Data Visualization', 'Visualization techniques and best practices'),
    (2, (SELECT Id FROM FlashCardCollections WHERE UserId = 2 AND Title = 'Data Science & ML'), 'Deep Learning', 'Neural networks and deep learning concepts');

-- =============================================
-- FlashCards for User 1 Collections
-- =============================================

-- Python Basics (30 flashcards)
INSERT INTO FlashCards (Term, Definition, Score, TimesLearned, FlashCardCollectionId)
VALUES 
    ('list', 'An ordered, mutable collection of items in Python. Created using square brackets []', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('tuple', 'An ordered, immutable collection of items in Python. Created using parentheses ()', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('dictionary', 'An unordered collection of key-value pairs in Python. Created using curly braces {}', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('set', 'An unordered collection of unique items in Python. Created using curly braces {} or set()', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('lambda', 'A small anonymous function defined with the lambda keyword. Can take any number of arguments but has only one expression', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('list comprehension', 'A concise way to create lists in Python using the syntax: [expression for item in iterable if condition]', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('def', 'Keyword used to define a function in Python', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('class', 'Keyword used to define a class in Python, which is a blueprint for creating objects', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('__init__', 'Special method (constructor) in Python classes that is automatically called when an object is created', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('self', 'Reference to the current instance of a class in Python. Used as the first parameter in instance methods', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('import', 'Keyword used to import modules or specific functions/classes from modules in Python', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('with', 'Context manager keyword that ensures proper acquisition and release of resources (like file handles)', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('try-except', 'Exception handling structure in Python used to catch and handle errors gracefully', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('for loop', 'Iteration statement that loops over a sequence (list, tuple, string, etc.) in Python', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('while loop', 'Loop that continues executing as long as a condition is True in Python', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('if-elif-else', 'Conditional statement structure in Python for making decisions based on conditions', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('range()', 'Built-in function that generates a sequence of numbers. Commonly used in for loops', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('len()', 'Built-in function that returns the length (number of items) of an object', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('print()', 'Built-in function used to output text or variables to the console', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('input()', 'Built-in function that reads a line of input from the user as a string', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('str()', 'Built-in function that converts a value to a string', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('int()', 'Built-in function that converts a value to an integer', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('float()', 'Built-in function that converts a value to a floating-point number', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('bool()', 'Built-in function that converts a value to a boolean (True or False)', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('None', 'Special constant in Python that represents the absence of a value or a null value', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('pass', 'Null statement in Python used as a placeholder when a statement is syntactically required but no code needs to execute', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('return', 'Statement used to exit a function and optionally pass back a value to the caller', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('break', 'Statement used to exit a loop prematurely in Python', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('continue', 'Statement used to skip the rest of the current iteration and move to the next iteration in a loop', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics')),
    ('yield', 'Keyword used in generator functions to produce a value and pause function execution', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Python Basics'));

-- JavaScript ES6 (30 flashcards)
INSERT INTO FlashCards (Term, Definition, Score, TimesLearned, FlashCardCollectionId)
VALUES 
    ('let', 'Block-scoped variable declaration keyword introduced in ES6. Can be reassigned', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('const', 'Block-scoped constant declaration keyword in ES6. Cannot be reassigned after initialization', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('arrow function', 'Compact function syntax using => that also lexically binds the this value', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('template literals', 'String literals allowing embedded expressions using backticks and ${expression} syntax', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('destructuring', 'Syntax for extracting values from arrays or properties from objects into distinct variables', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('spread operator', 'Syntax (...) that expands an iterable into individual elements', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('rest parameters', 'Syntax (...args) that collects all remaining arguments into an array', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('default parameters', 'Function parameters that have default values if no argument is passed', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Promise', 'Object representing the eventual completion or failure of an asynchronous operation', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('async/await', 'Syntax for writing asynchronous code that looks synchronous using async functions and await keyword', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('class', 'ES6 syntax for defining object-oriented classes as a cleaner alternative to prototypes', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('import/export', 'ES6 module system for importing and exporting functions, objects, or values between files', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Map', 'Collection of keyed data items that can use any type as a key, unlike objects', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Set', 'Collection that stores unique values of any type', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Symbol', 'Unique and immutable primitive value that can be used as object property keys', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('for...of', 'Loop that iterates over iterable objects (arrays, strings, maps, sets, etc.)', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Array.map()', 'Method that creates a new array with the results of calling a function on every element', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Array.filter()', 'Method that creates a new array with all elements that pass a test function', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Array.reduce()', 'Method that executes a reducer function on each array element, resulting in a single output value', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Array.find()', 'Method that returns the first element in an array that satisfies a test function', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Object.assign()', 'Method used to copy properties from source objects to a target object', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Object.keys()', 'Method that returns an array of a given object''s own enumerable property names', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Object.values()', 'Method that returns an array of a given object''s own enumerable property values', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Object.entries()', 'Method that returns an array of a given object''s own enumerable [key, value] pairs', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Array.includes()', 'Method that checks if an array contains a certain value, returning true or false', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('String.includes()', 'Method that checks if a string contains a specified substring, returning true or false', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('String.startsWith()', 'Method that checks if a string starts with specified characters, returning true or false', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('String.endsWith()', 'Method that checks if a string ends with specified characters, returning true or false', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('String.repeat()', 'Method that returns a new string with a specified number of copies of the original string', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6')),
    ('Array.from()', 'Method that creates a new array from an array-like or iterable object', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'JavaScript ES6'));

-- C# Fundamentals (30 flashcards)
INSERT INTO FlashCards (Term, Definition, Score, TimesLearned, FlashCardCollectionId)
VALUES 
    ('namespace', 'Organizational container for classes and other types in C#. Helps avoid naming conflicts', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('class', 'Blueprint for creating objects that encapsulates data and behavior in C#', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('interface', 'Contract that defines a set of methods and properties that implementing classes must provide', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('struct', 'Value type in C# that can contain data and related functionality, typically for small data structures', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('enum', 'Value type that defines a set of named constants in C#', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('public', 'Access modifier that makes a member accessible from any code', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('private', 'Access modifier that restricts access to members only within the containing class', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('protected', 'Access modifier that allows access within the containing class and derived classes', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('static', 'Modifier that makes a member belong to the type itself rather than to instances of the type', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('virtual', 'Modifier that allows a method or property to be overridden in derived classes', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('override', 'Modifier used to provide a new implementation of a virtual member inherited from a base class', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('abstract', 'Modifier that indicates a class cannot be instantiated or a member must be implemented in derived classes', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('sealed', 'Modifier that prevents a class from being inherited or a method from being overridden', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('var', 'Implicitly typed local variable keyword where the compiler infers the type from the initialization expression', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('string', 'Reference type representing a sequence of Unicode characters in C#', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('int', 'Value type representing a 32-bit signed integer in C#', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('bool', 'Value type representing a Boolean value (true or false) in C#', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('null', 'Literal representing a null reference, meaning no object is referenced', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('List<T>', 'Generic collection class that represents a strongly typed list of objects that can be accessed by index', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('Dictionary<TKey, TValue>', 'Generic collection of key-value pairs in C#', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('LINQ', 'Language Integrated Query - set of methods for querying data from various sources in C#', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('lambda expression', 'Anonymous function using => syntax, commonly used with LINQ and delegates', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('async/await', 'Keywords for writing asynchronous code that is easier to read and maintain in C#', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('Task', 'Represents an asynchronous operation in C#. Base type for async methods', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('try-catch-finally', 'Exception handling structure where try contains code, catch handles exceptions, and finally runs cleanup code', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('using statement', 'Ensures proper disposal of IDisposable objects when they go out of scope', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('property', 'Member that provides a flexible mechanism to read, write, or compute the value of a private field', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('constructor', 'Special method that is called when an object is created, used to initialize object state', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('this', 'Keyword that refers to the current instance of a class', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals')),
    ('base', 'Keyword used to access members of the base class from within a derived class', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'C# Fundamentals'));

-- SQL Queries (30 flashcards)
INSERT INTO FlashCards (Term, Definition, Score, TimesLearned, FlashCardCollectionId)
VALUES 
    ('SELECT', 'SQL statement used to retrieve data from one or more tables', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('FROM', 'Clause that specifies the table(s) from which to retrieve data', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('WHERE', 'Clause used to filter records based on specified conditions', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('JOIN', 'Combines rows from two or more tables based on a related column between them', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('INNER JOIN', 'Returns records that have matching values in both tables', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('LEFT JOIN', 'Returns all records from the left table and matched records from the right table', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('RIGHT JOIN', 'Returns all records from the right table and matched records from the left table', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('FULL OUTER JOIN', 'Returns all records when there is a match in either left or right table', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('GROUP BY', 'Groups rows that have the same values in specified columns into summary rows', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('HAVING', 'Filters groups based on a specified condition, used with GROUP BY', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('ORDER BY', 'Sorts the result set in ascending (ASC) or descending (DESC) order', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('INSERT INTO', 'Adds new records into a table', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('UPDATE', 'Modifies existing records in a table', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('DELETE', 'Removes existing records from a table', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('CREATE TABLE', 'Creates a new table in the database with specified columns and data types', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('ALTER TABLE', 'Modifies an existing table structure (add, delete, or modify columns)', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('DROP TABLE', 'Deletes an entire table and all its data from the database', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('PRIMARY KEY', 'Constraint that uniquely identifies each record in a table', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('FOREIGN KEY', 'Constraint that establishes a relationship between two tables', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('UNIQUE', 'Constraint that ensures all values in a column are different', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('NOT NULL', 'Constraint that ensures a column cannot have a NULL value', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('COUNT()', 'Aggregate function that returns the number of rows that match a specified criterion', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('SUM()', 'Aggregate function that returns the total sum of a numeric column', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('AVG()', 'Aggregate function that returns the average value of a numeric column', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('MAX()', 'Aggregate function that returns the largest value of a selected column', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('MIN()', 'Aggregate function that returns the smallest value of a selected column', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('DISTINCT', 'Keyword used to return only distinct (unique) values', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('LIKE', 'Operator used in a WHERE clause to search for a specified pattern using wildcards', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('IN', 'Operator that allows you to specify multiple values in a WHERE clause', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries')),
    ('BETWEEN', 'Operator that selects values within a given range (inclusive)', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'SQL Queries'));

-- =============================================
-- FlashCards for User 2 Collections
-- =============================================

-- Machine Learning Algorithms (30 flashcards)
INSERT INTO FlashCards (Term, Definition, Score, TimesLearned, FlashCardCollectionId)
VALUES 
    ('Linear Regression', 'Supervised learning algorithm that models the relationship between a dependent variable and one or more independent variables using a linear equation', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Logistic Regression', 'Classification algorithm that predicts the probability of a binary outcome using the logistic function', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Decision Tree', 'Tree-structured model that makes decisions by splitting data based on feature values', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Random Forest', 'Ensemble learning method that builds multiple decision trees and combines their predictions', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('K-Nearest Neighbors (KNN)', 'Classification algorithm that assigns a class based on the majority class of k nearest neighbors', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Support Vector Machine (SVM)', 'Supervised learning algorithm that finds the optimal hyperplane to separate different classes', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Naive Bayes', 'Probabilistic classifier based on Bayes'' theorem with the assumption of feature independence', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('K-Means Clustering', 'Unsupervised learning algorithm that partitions data into k clusters based on feature similarity', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Gradient Boosting', 'Ensemble technique that builds models sequentially, with each new model correcting errors of previous ones', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('XGBoost', 'Optimized gradient boosting library designed for speed and performance', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('AdaBoost', 'Adaptive boosting algorithm that combines multiple weak classifiers to create a strong classifier', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Principal Component Analysis (PCA)', 'Dimensionality reduction technique that transforms data into a lower-dimensional space while preserving variance', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Neural Network', 'Computing system inspired by biological neural networks, consisting of interconnected nodes (neurons)', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Overfitting', 'When a model learns training data too well, including noise, resulting in poor performance on new data', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Underfitting', 'When a model is too simple to capture the underlying pattern in the data', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Cross-Validation', 'Technique for assessing model performance by partitioning data and training/testing on different subsets', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Feature Engineering', 'Process of creating new features or transforming existing ones to improve model performance', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Regularization', 'Technique to prevent overfitting by adding a penalty term to the loss function', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('L1 Regularization (Lasso)', 'Adds absolute value of coefficients as penalty term, can lead to sparse models', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('L2 Regularization (Ridge)', 'Adds squared magnitude of coefficients as penalty term, shrinks coefficients towards zero', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Confusion Matrix', 'Table used to evaluate classification performance showing true/false positives/negatives', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Precision', 'Metric measuring the proportion of true positive predictions among all positive predictions', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Recall (Sensitivity)', 'Metric measuring the proportion of true positives correctly identified among all actual positives', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('F1 Score', 'Harmonic mean of precision and recall, providing a balanced measure of model performance', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('ROC Curve', 'Graph showing the performance of a classification model at all classification thresholds', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('AUC (Area Under Curve)', 'Metric measuring the entire two-dimensional area underneath the ROC curve', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Ensemble Learning', 'Technique that combines multiple models to produce better predictions than individual models', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Bagging', 'Ensemble method that trains multiple models on different random samples and averages predictions', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Boosting', 'Ensemble method that trains models sequentially, with each model focusing on errors of previous ones', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms')),
    ('Hyperparameter Tuning', 'Process of finding optimal values for model parameters that are not learned from data', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Machine Learning Algorithms'));

-- Statistics Basics (30 flashcards)
INSERT INTO FlashCards (Term, Definition, Score, TimesLearned, FlashCardCollectionId)
VALUES 
    ('Mean', 'The average value calculated by summing all values and dividing by the count', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Median', 'The middle value in an ordered dataset, separating the higher half from the lower half', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Mode', 'The value that appears most frequently in a dataset', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Standard Deviation', 'Measure of the amount of variation or dispersion in a set of values', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Variance', 'The average of the squared differences from the mean', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Range', 'The difference between the maximum and minimum values in a dataset', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Quartiles', 'Values that divide a dataset into four equal parts (Q1, Q2/median, Q3)', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Interquartile Range (IQR)', 'The range between the first quartile (Q1) and third quartile (Q3)', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Outlier', 'A data point that differs significantly from other observations', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Normal Distribution', 'Symmetric, bell-shaped probability distribution where most values cluster around the mean', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Skewness', 'Measure of the asymmetry of a probability distribution around its mean', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Kurtosis', 'Measure of the "tailedness" of a probability distribution', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Correlation', 'Measure of the strength and direction of a linear relationship between two variables', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Correlation Coefficient', 'A value between -1 and 1 indicating the strength of a linear relationship (Pearson''s r)', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Covariance', 'Measure of how much two variables change together', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Probability', 'Measure of the likelihood that an event will occur, expressed as a number between 0 and 1', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Random Variable', 'A variable whose value is subject to variations due to chance', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Expected Value', 'The long-run average value of a random variable over many repetitions', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Binomial Distribution', 'Discrete probability distribution of the number of successes in a fixed number of independent trials', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Poisson Distribution', 'Discrete probability distribution expressing the probability of a given number of events in a fixed interval', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Hypothesis Testing', 'Statistical method for making decisions about population parameters based on sample data', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Null Hypothesis', 'Default assumption that there is no effect or no difference in hypothesis testing', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Alternative Hypothesis', 'Statement that contradicts the null hypothesis, suggesting an effect or difference exists', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('P-value', 'Probability of obtaining results at least as extreme as observed, assuming the null hypothesis is true', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Significance Level (α)', 'Threshold probability for rejecting the null hypothesis, commonly set at 0.05', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Type I Error', 'Rejecting the null hypothesis when it is actually true (false positive)', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Type II Error', 'Failing to reject the null hypothesis when it is actually false (false negative)', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Confidence Interval', 'Range of values that likely contains the true population parameter with a specified confidence level', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Sample Size', 'The number of observations or data points included in a statistical sample', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics')),
    ('Central Limit Theorem', 'States that the distribution of sample means approximates a normal distribution as sample size increases', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Statistics Basics'));

-- Data Visualization (30 flashcards)
INSERT INTO FlashCards (Term, Definition, Score, TimesLearned, FlashCardCollectionId)
VALUES 
    ('Bar Chart', 'Graph using rectangular bars to show comparisons between categories, with bar length proportional to values', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Line Chart', 'Graph that displays information as a series of data points connected by straight lines, ideal for showing trends over time', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Scatter Plot', 'Graph that uses dots to represent values for two variables, showing relationships or correlations', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Histogram', 'Graph that displays the distribution of numerical data using bars representing frequency within intervals', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Pie Chart', 'Circular chart divided into slices to show proportions or percentages of a whole', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Box Plot', 'Standardized way of displaying data distribution using quartiles, showing median, IQR, and outliers', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Heatmap', 'Data visualization technique that uses color to represent values in a matrix format', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Violin Plot', 'Combination of box plot and density plot showing probability density at different values', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Area Chart', 'Graph that displays quantitative data visually with the area between the axis and line filled with color', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Bubble Chart', 'Variation of scatter plot where a third dimension is represented by bubble size', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Treemap', 'Visualization that displays hierarchical data using nested rectangles, with size proportional to values', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Sunburst Chart', 'Hierarchical visualization using concentric circles, with each ring representing a level in the hierarchy', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Gantt Chart', 'Bar chart that illustrates project schedules, showing start and finish dates of elements', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Waterfall Chart', 'Chart that shows how an initial value is affected by a series of positive or negative values', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Radar Chart', 'Chart displaying multivariate data on axes starting from the same point, forming a spider web appearance', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Choropleth Map', 'Thematic map with areas shaded in proportion to the measurement of a statistical variable', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Sankey Diagram', 'Flow diagram where arrow width is proportional to flow quantity, showing resource transfer', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Word Cloud', 'Visual representation of text data where word size indicates frequency or importance', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Matplotlib', 'Comprehensive Python library for creating static, animated, and interactive visualizations', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Seaborn', 'Python visualization library based on matplotlib that provides a high-level interface for statistical graphics', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Plotly', 'Interactive graphing library for Python that creates browser-based visualizations', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('D3.js', 'JavaScript library for manipulating documents based on data using HTML, SVG, and CSS', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Tableau', 'Business intelligence and analytics platform for creating interactive visualizations and dashboards', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Power BI', 'Microsoft business analytics service for creating interactive reports and visualizations', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Color Theory', 'Science of how colors interact and the visual effects of color combinations in visualizations', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Data-Ink Ratio', 'Proportion of ink devoted to displaying actual data vs. non-data elements (Tufte''s principle)', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Gestalt Principles', 'Principles describing how humans perceive visual elements as organized patterns or wholes', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Dashboard', 'Visual display of the most important information needed to achieve objectives, consolidated on a single screen', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Interactive Visualization', 'Data visualization that allows users to interact with data through filtering, zooming, or selecting', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization')),
    ('Infographic', 'Visual representation of information combining graphics, text, and data to tell a story or explain concepts', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Data Visualization'));

-- Deep Learning (30 flashcards)
INSERT INTO FlashCards (Term, Definition, Score, TimesLearned, FlashCardCollectionId)
VALUES 
    ('Artificial Neural Network', 'Computing system inspired by biological neural networks that learn to perform tasks by considering examples', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Deep Learning', 'Subset of machine learning using neural networks with multiple layers to learn hierarchical representations', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Neuron/Node', 'Basic unit in a neural network that receives inputs, applies weights and activation function, and produces output', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Layer', 'Collection of neurons in a neural network that process inputs at the same depth level', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Input Layer', 'First layer of a neural network that receives the raw input data', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Hidden Layer', 'Intermediate layers between input and output that extract features and learn representations', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Output Layer', 'Final layer of a neural network that produces the prediction or classification result', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Weight', 'Parameter that determines the strength of the connection between neurons', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Bias', 'Additional parameter in neurons that shifts the activation function', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Activation Function', 'Non-linear function applied to neuron output to introduce non-linearity into the network', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('ReLU', 'Rectified Linear Unit activation function: f(x) = max(0, x), most commonly used in hidden layers', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Sigmoid', 'Activation function that maps inputs to values between 0 and 1: f(x) = 1 / (1 + e^-x)', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Softmax', 'Activation function that converts raw scores into probabilities that sum to 1, used in output layer for classification', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Tanh', 'Hyperbolic tangent activation function that maps inputs to values between -1 and 1', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Forward Propagation', 'Process of passing input data through the network layers to generate output predictions', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Backpropagation', 'Algorithm for training neural networks by computing gradients and updating weights backward through layers', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Loss Function', 'Function that measures how well the network''s predictions match the actual targets', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Gradient Descent', 'Optimization algorithm that iteratively adjusts parameters to minimize the loss function', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Learning Rate', 'Hyperparameter that controls the step size when updating weights during training', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Batch Size', 'Number of training examples processed together before updating model parameters', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Epoch', 'One complete pass through the entire training dataset during neural network training', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Dropout', 'Regularization technique that randomly sets a fraction of neurons to zero during training to prevent overfitting', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Batch Normalization', 'Technique that normalizes inputs to each layer, accelerating training and improving stability', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Convolutional Neural Network (CNN)', 'Neural network architecture designed for processing grid-like data such as images', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Recurrent Neural Network (RNN)', 'Neural network architecture with loops allowing information to persist, used for sequential data', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('LSTM', 'Long Short-Term Memory network - type of RNN designed to remember long-term dependencies', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('GRU', 'Gated Recurrent Unit - simplified version of LSTM with fewer parameters', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Transfer Learning', 'Technique where a model trained on one task is reused as starting point for a related task', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Attention Mechanism', 'Technique that allows models to focus on specific parts of input when producing output', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning')),
    ('Transformer', 'Neural network architecture based entirely on attention mechanisms, foundation for models like BERT and GPT', 0, 0, (SELECT Id FROM FlashCardCollections WHERE Title = 'Deep Learning'));

-- =============================================
-- Script Completion Message
-- =============================================
GO
PRINT 'Data initialization completed successfully!'
PRINT 'Total Collections Created: 10 (5 per user)'
PRINT 'Total FlashCards Created: 240 (30 per leaf collection × 8 leaf collections)'
PRINT 'User 1 has 4 leaf collections with flashcards'
PRINT 'User 2 has 4 leaf collections with flashcards'
