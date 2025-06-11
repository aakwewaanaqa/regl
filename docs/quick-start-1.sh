#!/bin/bash

# As you executed this script
# you'll see the class was completely copied to output
# By catching the output,
# you can generate another file for end users.

  < quick-start-1.src.cs regl gen

# To generate output simply adds '>' to redirect output
#                                 ↓
  < quick-start-1.src.cs regl gen > gen.cs

