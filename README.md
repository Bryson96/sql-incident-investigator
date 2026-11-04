# SQL Incident Investigator

## The problem

When something breaks in a database, most of the first hour of investigation is legwork:
- checking whether the team has seen the problem before,
- working out which tables matter, and
- running query after query to rule ideas in or out.

This project hands that legwork to an AI. A person still confirms the cause and decides the fix.

## How it works

You ask a question in Claude Desktop, such as *"Customers say orders from April never shipped. Why?"*
The AI then:

1. **Searches team notes** for runbooks and similar past incidents.
2. **Looks up the tables** it needs, using tools from a small C# server.
3. **Runs read-only queries**, testing one idea at a time, guided by project instructions.
4. **Repeats on its own** until it can name the root cause, the evidence, and what a person still needs to confirm.

The AI connects with a database login that can only read, so it can't change any data.
