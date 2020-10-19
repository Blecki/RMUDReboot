antechamber : ROOM(INTERIOR) {
	SecretPassageOpen? = NO;
	[ambient light] = BRIGHT;
	SHORT = "Palantine Villa - Antechamber";
    LONG = "Two imposing statues stand guard in this small room, on either side of the door to the room beyond. On the left, Jupiter, king of the gods. On the right, Minerva, the goddess of wisdom and beauty.";

	MOVE(jupiter, HERE, SCENERY);
	MOVE(minerva, HERE, SCENERY);
	MOVE(hammer, HERE);
	MOVE(table, HERE);
	MOVE([old vase], table, ON);
	MOVE("palantine/ball", HERE);

	LINK(NORTH, "palantine/disambig");
	LINK(EAST, "palantine/solar");
	LINK(WEST, "palantine/garden");
}

[old vase] {
	SHORT = "old vase";
	LONG = "An old, cracked vase.";
	NOUNS = "old", "vase";
}

minerva {
	SHORT = "minerva";
	LONG = "Minerva is turned to regard her father Jupiter, and poses with one hand on her hips and the other on the shaft of a massive hammer.";
	NOUNS = "minerva";
}

hammer {
	CHECK "can pull?" {
		ALLOW;
	}

	PERFORM "pull" {
		if (antechamber.SecretPassageOpen?)
			ACTOR <= "The hammer doesn't budge.";
		else {
			antechamber.SecretPassageOpen? = YES;
			antechamber.LINK(SOUTH, "palantine/pit");
			ACTOR.LOCATION <= "The hammer slides up a few inches, then lodges firmly in place. With a rumble, a section of the south wall slides aside.");
		}
		STOP;
	}

	LONG = "It's really quite impressive. Despite the hammer's massive size, Minerva's grip is rather dainty. You could pull the hammer right out.";
	NOUNS = "hammer", "shaft";
}

jupiter {
	"scenery?" = YES;
	NOUNS = "jupiter";
	LONG = "Jupiter holds in his left hand a gleaming thunderbolt. It glows bright enough to light the entire chamber. In his right, he holds a chisel.";

	VALUE "light level" {
		BRIGHT;
	}
}

table {
	CONTAINER(ON | UNDER, ON);
	SHORT = "ancient table";
	LONG = "As the years have worn long the wood of this table has dried and shrunk, and split, and what was once a finely crafted table is now pitted and gouged. The top is still mostly smooth, from use but not from care.";
	NOUNS = "ancient", "table";
	ARTICLE = "an";
	MOVE(matchbook, here, UNDER);

	CHECK "can take?" only this {
		ACTOR <= "It's far too heavy";
		DISALLOW;
	}

	CHECK "can push direction?" { ALLOW; }
}

matchbook {
	SHORT = "matchbook";
	LONG = "A small book of matches with a thunderbolt on the cover.";
	NOUNS = "matchbook", "book";
}